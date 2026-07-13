using Data.Contracts;
using Entites.Orders;
using Microsoft.EntityFrameworkCore;
using Services.OrderService.Site.ApplyDiscountCode;

namespace Services.OrderService.Site.GetPayRequest
{
    public class GetRequestServic : IGetRequestServic
    {
        private readonly IRepository<RequestPay> _requestPayRepository;

        private readonly IApplyDiscountCodeService _applyDiscountCodeService;


        public GetRequestServic(IRepository<RequestPay>repository, IApplyDiscountCodeService applyDiscountCodeService)
        {
            _requestPayRepository = repository; 
            _applyDiscountCodeService = applyDiscountCodeService;
        }
        public async Task<RequestPay> Execute(Guid guid, CancellationToken cancellationToken)
        {
            var result = await _requestPayRepository.TableNoTracking.Include(x=>x.Order).ThenInclude(x=>x.OrderDetails).Where(x => x.Guid == guid).FirstOrDefaultAsync();

            if (result.Order.DiscountCode!="")
            {
                var resultdiscount =await _applyDiscountCodeService.Execute(new ApplyDiscountCodeRequest 
                {
                Code=result.Order.DiscountCode,
                Items=result.Order.OrderDetails.Select(x=>new ApplyDiscountItemRequest
                {
                    Count=x.Count,
                    ProductId=x.ProductId
                
                }).ToList()
                },cancellationToken);

                result.Amount = (long)resultdiscount.FinalPrice;
            }
      
         
            
            return result??new RequestPay();
        }

       

    }
}
