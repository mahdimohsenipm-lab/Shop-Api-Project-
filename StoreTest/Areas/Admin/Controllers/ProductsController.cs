using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.DiscountService.Area.AddDiscountService;
using Services.ProductsServices.Commands.AddProduct;
using Services.ProductsServices.Commands.UpdateProduct;
using Services.ProductsServices.Querys.GetProductDetail;
using Services.ViewModel.Area.Model.Dto;
using Services.ViewModel.Area.Model.Request;
using WebFramework.Filter;
using X.PagedList.Extensions;

namespace StoreTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiResultFilter]
    [Authorize(Roles = "Admin")]

    public class ProductsController : Controller
    {
        private readonly IProductsRepository product;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAddProductService addProductService;
        private readonly IGetProductDetailService getProductDetail;
        private readonly IUpdateProductService updateProductService;
        private readonly IMapper mapper;
        private readonly IAddDiscountService addDiscountService;
        public ProductsController(IProductsRepository products, IAddProductService addProductService,
            IMapper mapper,ICategoryRepository categoryRepository, 
            IGetProductDetailService getProductDetail, 
            IUpdateProductService updateProductService, IAddDiscountService addDiscountService)
        {
            product = products;
            this.mapper = mapper;
            this.addProductService = addProductService;
            this.categoryRepository = categoryRepository;
            this.getProductDetail= getProductDetail;
            this.updateProductService = updateProductService;
            this.addDiscountService = addDiscountService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            var now = DateTimeOffset.UtcNow;
            var products = product.TableNoTracking.Select(x=> new ProductDto
            {
                Inventory=x.Inventory,
                Brand=x.Brand,
                Price=x.Price,
                Id=x.Id,
                Name=x.Name,
                IsDiscount = x.ProductDiscounts.Any(d =>
           d.IsActive &&
           d.StartTime <= now &&
           d.EndTime >= now)


            }).ToPagedList(page??1,2);
            

            var result = new RequestProductDto
            {
                Products= products
            };

            return View(result);

        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestAddProducts dto,List<ProductFitureRequest> productFiture , CancellationToken cancellationToken)
        {

            List<IFormFile> image = new List<IFormFile>();

            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                image.Add(file);

            }
            dto.Image = image;
            dto.Features = productFiture;
           await addProductService.addProducts(dto, cancellationToken);
            return Ok();

        }
        [HttpGet]
        public IActionResult Create()
        {

            var data = categoryRepository.TableNoTracking.Select(x => new { x.Id, x.Name }).ToList();
            ViewBag.Categories = new SelectList(data ?? Enumerable.Empty<object>(), "Id", "Name");
            return View();


        }
        [HttpGet]
        public async Task<IActionResult> Detail(int Id , CancellationToken cancellationToken)
        {
         var result=  await getProductDetail.Execute(Id,cancellationToken);
            return View(result);


        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            await product.DeleteProducts(id, cancellationToken);
            return Ok();


        }

        [HttpPost]
        public async Task<IActionResult> Update(int Id,UpdateRequestDto requestDto,CancellationToken cancellationToken)
        {
          await updateProductService.Execute(Id,requestDto,cancellationToken);
            return Ok();


        }

        [HttpPost]
        public async Task<IActionResult> AddDiscount(RequestDiscount request,CancellationToken cancellationToken)
        {
            if (request==null)
            {
                return BadRequest("مقادیر را درست ارسال کنید");
            }
           
           await addDiscountService.Execute(cancellationToken,request);

            return Ok();
        }


    }


    
}
