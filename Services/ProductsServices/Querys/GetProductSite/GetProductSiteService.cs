using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Entites.Products;
using Microsoft.EntityFrameworkCore;
using Services.ProductsServices.Querys.GetProductSite.PriceCalculator;

namespace Services.ProductsServices.Querys.GetProductSite
{
    public class GetProductSiteService : IGetProductSiteService
    {
        private readonly IProductsRepository productsRepository;
        private readonly IMapper mapper;
        private readonly IPriceCalculatorService priceCalculatorService;


        public GetProductSiteService(IProductsRepository productsRepository
            , IMapper mapper, IPriceCalculatorService priceCalculatorService)
        {
            this.productsRepository = productsRepository;
            this.mapper = mapper;
            this.priceCalculatorService = priceCalculatorService;

        }
        public async Task<ResultProductSiteDto> Execute(CancellationToken cancellationToken)
        {
            var now=DateTimeOffset.Now;
            var products = await productsRepository
                .TableNoTracking
                .Where(x => x.Displayed == true).Select(x => new ProductSiteDto
                {
                    Amount = x.ProductDiscounts.Where(x => x.IsActive == true && x.StartTime <= now && x.EndTime >= now)
                    .OrderByDescending(x => x.StartTime).Select(x=>x.Amount.Value).FirstOrDefault(),
                    Percentage=x.ProductDiscounts.Where(x=>x.IsActive== true &&x.StartTime <= now && x.EndTime >= now)
                    .OrderByDescending(x=>x.StartTime).Select(x=>x.Percentage.Value).FirstOrDefault(),
                 id=x.Id,
                 Name=x.Name,
                 Price=x.Price,
                 Src=x.ProductImages.Select(x=>x.Src).FirstOrDefault(),
                 
                 
                }).Take(6).ToListAsync();

            foreach (var item in products)
            {
                item.FinalPrice = priceCalculatorService.Calculate(new ProductDiscountDto 
                {Amount=item.Amount,Percentage=item.Percentage },item.Price);
            }


            var results = new ResultProductSiteDto
            {
                Products = products, 
            };

            return results;


        }
    }
}
