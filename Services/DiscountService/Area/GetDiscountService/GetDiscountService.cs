using Data.Contracts;
using Entites.Products;
using Microsoft.EntityFrameworkCore;
using Services.ViewModel.Area.Model.Dto;
using Services.ViewModel.Area.Model.Request;
using X.PagedList.Extensions;

namespace Services.DiscountService.Area.GetDiscountService
{
    public class GetDiscountService : IGetDiscountService
    {
        private readonly IRepository<ProductDiscount> _repository;

        public GetDiscountService(IRepository<ProductDiscount> repository,
            IProductsRepository productsRepository)
        {
            _repository = repository;
        }
        public  RequestGetDiscount Execute(int? page)
        {
            var result = _repository.TableNoTracking.Include(x => x.Product).Select(x=>new DiscountDto
            { 
                Amount = x.Amount,
                Id=x.Id,
                IsActive=x.IsActive,
                Percentage=x.Percentage,
                ProductName=x.Product.Name,
                ProductId=x.ProductId
            
            }).ToPagedList(page??1,2);
            var finalResult = new RequestGetDiscount
            {
                Discounts=result
            };

            return finalResult;
           
        }
    }
}
