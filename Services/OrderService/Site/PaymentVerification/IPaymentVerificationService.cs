using Azure.Core;
using Data.Contracts;
using Entites.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Site.PaymentVerification
{
    public interface IPaymentVerificationService
    {
        Task Execute(Guid guid,long refid,string authority);
    }
    public class PaymentVerificationService : IPaymentVerificationService
    {
        private readonly IRepository<RequestPay> _requestPayRepository;

        public PaymentVerificationService(IRepository<RequestPay> repository)
        {
            _requestPayRepository = repository;
        }
        public async Task Execute(Guid guid, long refid,string aturity)
        {
            var result =await _requestPayRepository.Table.Include(x => x.Order).FirstOrDefaultAsync(x=>x.Guid==guid);

            if (result == null)
            {
                throw new Exception("مشکلی پیش امده");
            
            }
            result.PayDate = DateTime.Now;
            result.IsPay = true;
            result.Order.orderState = OrderState.Success;
            result.RefId = refid;
            result.Authority = aturity;
             _requestPayRepository.Update(result);

        }
    }
}
