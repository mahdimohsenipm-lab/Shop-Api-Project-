using Data.Contracts;
using Entites.Products;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Services.DiscountService.Area.GetDiscountDetail
{
    public class GetDiscountDetailService : IGetDiscountDetailService
    {
        private readonly IRepository<ProductDiscount> repository;

        public GetDiscountDetailService(IRepository<ProductDiscount> repository)
        {
            this.repository = repository;
        }
        public async Task<ProductDiscountDetaliDto> Execute(int Id,bool? index, CancellationToken cancellationToken)
        {
            var result = await repository.TableNoTracking
            .Include(x => x.Product)
            .Where(x => x.ProductId == Id && x.IsActive)
            .Select(x => new ProductDiscountDetaliDto
            {
               AmountDiscount = x.Amount,
               Id = x.Id,
               ProductName=x.Product.Name,
               PercentageDiscount = x.Percentage,
               ProductPrice=x.Product.Price,
               index= index?? false,
               FinalPrice = x.Amount > 0?x.Product.Price - x.Amount.Value : x.Product.Price * (1-x.Percentage.Value / 100m),
               EndTime=x.EndTime

            }).FirstOrDefaultAsync();

            //if (result == null) 
            //{throw new Exception("موردی یافت نشد");};

            return result;
        }
     


    }
}
