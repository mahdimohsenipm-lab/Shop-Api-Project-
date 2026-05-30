using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Services.ProductsServices.Querys.GetProductSite
{
    public class GetProductSiteService : IGetProductSiteService
    {
        private readonly IProductsRepository productsRepository;
        private readonly IMapper mapper;


        public GetProductSiteService(IProductsRepository productsRepository, IMapper mapper)
        {
            this.productsRepository = productsRepository;
            this.mapper = mapper;

        }
        public async Task<ResultProductSiteDto> Execute(CancellationToken cancellationToken)
        {
            var products =await  productsRepository.TableNoTracking.Where(x=>x.Displayed==true)
                .Include(x=>x.ProductImages)
                .ProjectTo<ProductSiteDto>(mapper.ConfigurationProvider)
                .Take(6).ToListAsync(cancellationToken);

            var results = new ResultProductSiteDto
            {
                Products = products, 
            };

            return results;


        }
    }
}
