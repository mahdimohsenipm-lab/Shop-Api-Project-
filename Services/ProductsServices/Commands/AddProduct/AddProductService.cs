using AutoMapper;
using Data.Contracts;
using Entites.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.ViewModel.Area.Model.Dto;

namespace Services.ProductsServices.Commands.AddProduct
{
    public class AddProductService : IAddProductService
    {
        private readonly IProductsRepository productsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IRepository<ProductFiture> productFitureRepository;
        private readonly IRepository<ProductImage> productImageRepository;
        private readonly IWebHostEnvironment environment;
        private readonly IMapper mapper;

        public AddProductService(IProductsRepository productsRepository
            , IMapper mapper, ICategoryRepository categoryRepository,
            IRepository<ProductFiture> productFitureRepository,
            IWebHostEnvironment environment,
            IRepository<ProductImage> productImageRepository)
        {
            this.productsRepository = productsRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.productFitureRepository = productFitureRepository;
            this.environment = environment;
            this.productImageRepository = productImageRepository;
        }
        public async Task addProducts(RequestAddProducts Productdto, CancellationToken cancellationToken)
        {
            var newproduct = mapper.Map<Product>(Productdto);

            var category = categoryRepository.GetById(Productdto.CategoryId);

            newproduct.Category = category;

         await productsRepository.AddAsync(newproduct, cancellationToken);

            List<ProductFiture> newProductFiture = new List<ProductFiture>();

            foreach (var item in Productdto.Features)
            {
                newProductFiture.Add(new ProductFiture
                {
                    Description = item.Valeu,
                    Product = newproduct,
                    Title = item.Title,
                    

                });
            }
         await   productFitureRepository.AddRangeAsync(newProductFiture,cancellationToken);


            List<ProductImage> images = new List<ProductImage>();

            var uploadedFiles = await UploadFilesAsync(Productdto.Image,cancellationToken);
            if (uploadedFiles==null)
            {
               throw new Exception("مشکلی در ارسال تصاویر پیش امده");
            }
            var image = uploadedFiles
         .Select(file => new ProductImage
         {
             Product = newproduct,
             Src = file.FileNameAddress
         })
         .ToList();


            await productImageRepository.AddRangeAsync(image, cancellationToken);

        }
        private async Task<List<UplodeFileRequest>> UploadFilesAsync(
     IEnumerable<IFormFile> files,
     CancellationToken cancellationToken)
        {
            const int maxFiles = 5;
            const long maxFileSize = 5 * 1024 * 1024;

            var allowedExtensions = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

            var allowedContentTypes = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

            var fileList = files?
                .Where(x => x is not null && x.Length > 0)
                .ToList()
                ?? new List<IFormFile>();

            if (fileList.Count == 0)
                return new List<UplodeFileRequest>();

            if (fileList.Count > maxFiles)
                throw new InvalidOperationException(
                    "حداکثر ۵ تصویر می‌توانید آپلود کنید.");

            // ابتدا تمام فایل‌ها را Validate می‌کنیم
            // تا اگر یکی مشکل داشت، هیچ فایلی ذخیره نشود.
            foreach (var file in fileList)
            {
                if (file.Length > maxFileSize)
                {
                    throw new InvalidOperationException(
                        $"حجم فایل {file.FileName} بیشتر از ۵ مگابایت است.");
                }

                var extension = Path.GetExtension(file.FileName);

                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException(
                        $"فرمت فایل {file.FileName} مجاز نیست.");
                }

                if (!allowedContentTypes.Contains(file.ContentType))
                {
                    throw new InvalidOperationException(
                        $"نوع فایل {file.FileName} معتبر نیست.");
                }
            }

            string folder = "images/ProductImages";

            string uploadsRootFolder = Path.Combine(
                environment.WebRootPath,
                folder);

            Directory.CreateDirectory(uploadsRootFolder);

            var uploadedFiles = new List<UplodeFileRequest>();

            try
            {
                foreach (var file in fileList)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var extension = Path.GetExtension(file.FileName)
                        .ToLowerInvariant();

                    var fileName = $"{Guid.NewGuid():N}{extension}";

                    var filePath = Path.Combine(
                        uploadsRootFolder,
                        fileName);

                    await using var fileStream = new FileStream(
                        filePath,
                        FileMode.CreateNew,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 81920,
                        useAsync: true);

                    await file.CopyToAsync(
                        fileStream,
                        cancellationToken);

                    uploadedFiles.Add(new UplodeFileRequest
                    {
                        Statuse = true,
                        FileNameAddress = $"{folder}/{fileName}"
                    });
                }

                return uploadedFiles;
            }
            catch
            {
                // اگر وسط آپلود خطایی رخ داد،
                // فایل‌هایی که تا اینجا ذخیره شده‌اند حذف می‌شوند.
                foreach (var file in uploadedFiles)
                {
                    var fileName = Path.GetFileName(file.FileNameAddress);

                    var filePath = Path.Combine(
                        uploadsRootFolder,
                        fileName);

                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }

                throw;
            }
        }

    }
}
