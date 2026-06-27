using AutoMapper;
using Data.Contracts;
using Entites.Products;
using Microsoft.EntityFrameworkCore;
using Services.ProductsServices.Querys.GetProductSite.PriceCalculator;

namespace Services.ProductsServices.Querys.GetProductDetailSite
{
    public class GetProductDetailSiteService : IGetProductDetailSiteService
    {
        private readonly IProductsRepository productsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly IRepository<ProductDiscount> discountRepository;
        private readonly IPriceCalculatorService priceCalculatorService;
        //in GetProductForSite

        public GetProductDetailSiteService(IProductsRepository productsRepository
            , IMapper mapper, ICategoryRepository categoryRepository
            , IRepository<ProductDiscount> discountRepository,
            IPriceCalculatorService priceCalculatorService)
        {
            this.productsRepository = productsRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.discountRepository = discountRepository;
            this.priceCalculatorService = priceCalculatorService;
        }
        public async Task<ProductDetailDto> Execute(int id, CancellationToken cancellationToken)
        {
            var product = await productsRepository.TableNoTracking
           .Include(x => x.ProductFitures)
           .Include(x => x.ProductImages)
           .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return null;

            var category = await categoryRepository.GetByIdAsync(cancellationToken, product.CategoriId);


            var discount = await discountRepository.TableNoTracking.OrderByDescending(x => x.StartTime)
            .Where(x => x.ProductId == product.Id).FirstOrDefaultAsync();

            var mapDto = mapper.Map<ProductDetailDto>(product);

            if (discount!=null)
            {
                mapDto.IsDiscount = true;
            }

            var finalPrice = priceCalculatorService.Calculate(new ProductDiscountDto
            {Amount=discount.Amount,Percentage=discount.Percentage },product.Price);

            mapDto.Amount = discount.Amount.Value;

            mapDto.Percentage = discount.Percentage.Value;

            mapDto.Category = category.Name;

            mapDto.FinalPrice = finalPrice;

            return mapDto;

        }
    }
}
