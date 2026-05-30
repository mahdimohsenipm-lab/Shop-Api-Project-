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

            foreach (var item in Productdto.Image)
            {
                var src = UploadFile(item);

                images.Add(new ProductImage()
                {
                    
                    Product = newproduct,
                    Src = src.FileNameAddress

                });

            }

           await productImageRepository.AddRangeAsync(images,cancellationToken);

        }
        private UplodeFileRequest UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new UplodeFileRequest
                {
                    Statuse = false,
                    FileNameAddress = ""
                };
            }

            string folder = "images/ProductImages/";
            var uploadsRootFolder = Path.Combine(environment.WebRootPath, folder);

            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            string extension = Path.GetExtension(file.FileName); // فقط پسوند
            string fileName = DateTime.Now.Ticks.ToString() + extension;
            var filePath = Path.Combine(uploadsRootFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return new UplodeFileRequest
            {
                FileNameAddress = Path.Combine(folder, fileName).Replace("\\", "/"),
                Statuse = true
            };
        }
     

    }
}
