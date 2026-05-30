using Data.Contracts;
using Entites.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Area.DeleteRequestPay
{
    public interface IDeleteRequestPayService
    {
        Task Execute(int id, CancellationToken cancellationToken);
    }

    public class DeleteRequestPayService : IDeleteRequestPayService
    {
        private readonly IRepository<RequestPay> repository;
        private readonly IRepository<Order> orderRepository;

        public DeleteRequestPayService(IRepository<RequestPay> repository, IRepository<Order> orderRepository)
        {
            this.repository = repository;
            this.orderRepository = orderRepository;
        }
        public async Task Execute(int id,CancellationToken cancellationToken)
        {
            //var request =await repository.Table.Include(x=>x.Order).ThenInclude(x=>x.OrderDetails).FirstOrDefaultAsync(x=>x.Id==id);
            //if (request==null)
            //{
            //    throw new Exception("مشکلی پیش امده");
            //}
            //var order = request.Order;

            var request =await repository.Table.FirstOrDefaultAsync(x=>x.Id==id);

            if (request == null)
            {
                throw new Exception("مشکلی پیش امده");
            }
            await repository.DeleteAsync(request,cancellationToken);



        }
    }
}
