using AutoMapper;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Services.ProductsServices.Querys.GetProductDetail
{
    public class GetProductDetailService : IGetProductDetailService
    {
        private readonly IProductsRepository products;
        private readonly ICategoryRepository category;
        private readonly IMapper mapper;
        public GetProductDetailService(IProductsRepository products,IMapper mapper,ICategoryRepository category)
        {
            this.products = products;
            this.mapper = mapper;
            this.category = category;
        }
        public async Task<ResultDtoProduct> Execute(int id, CancellationToken cancellationToken)
        {
            var product =await products.TableNoTracking.Include(x => x.ProductFitures).Include(x => x.ProductImages).FirstOrDefaultAsync(x=>x.Id==id);

            var categoryname =await category.GetByIdAsync(cancellationToken,product.CategoriId);

            var result=  mapper.Map<ResultDtoProduct>(product);

            result.Category = categoryname.Name;

            return result;


        }
    }
}
