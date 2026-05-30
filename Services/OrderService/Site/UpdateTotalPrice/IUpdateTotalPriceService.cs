using Data.Contracts;
using Entites.Orders;
using Entites.Users;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Site.UpdateTotalPrice
{
    public interface IUpdateTotalPriceService
    {
        Task Execute(int id,long totalprice, CancellationToken cancellationToken);
    }
    public class UpdateTotalPriceService : IUpdateTotalPriceService
    {
        private readonly IRepository<RequestPay> _repository;

        public UpdateTotalPriceService(IRepository<RequestPay> repository)
        {
                _repository= repository;
        }
        public async Task Execute(int id,long totalprice,CancellationToken cancellationToken)
        {
            var requestpay= await _repository.GetByIdAsync(cancellationToken,id);
            if (requestpay == null) 
            {
                throw new Exception("مشکلی پیش امده");
            
            }

            requestpay.Amount = totalprice;

            await _repository.UpdateAsync(requestpay,cancellationToken);


        }
    }
}
