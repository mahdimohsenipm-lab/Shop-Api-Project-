using Data.Contracts;
using Entites.Orders;
using Microsoft.EntityFrameworkCore;

namespace Services.OrderService.Site.GetPayRequest
{
    public class GetRequestServic : IGetRequestServic
    {
        private readonly IRepository<RequestPay> _requestPayRepository;

        public GetRequestServic(IRepository<RequestPay>repository)
        {
            _requestPayRepository = repository; 
        }
        public async Task<RequestPay> Execute(Guid guid)
        {
            var result = await _requestPayRepository.TableNoTracking.Where(x => x.Guid == guid).FirstOrDefaultAsync();
      
          
            
            return result??new RequestPay();
        }

       
    }
}
