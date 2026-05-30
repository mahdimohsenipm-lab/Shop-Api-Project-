using Data.Contracts;
using Entites.Orders;
using Microsoft.EntityFrameworkCore;

namespace Services.OrderService.Site.GetOrder
{
    public class GetOrderService : IGetOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public GetOrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Order Execute(int RequestPayId)
        {
            //var order = _orderRepository.Table.Select(x=> new Order 
            //{
            //    Address = x.Address,
            //    OrderDetails=x.OrderDetails,
            //    RequestPayId=x.RequestPayId,
            //    RequestPay=x.RequestPay,

            //}).FirstOrDefault(x=>x.UserId==userId);

            var order = _orderRepository.TableNoTracking
              .Where(x => x.RequestPayId == RequestPayId)
              .Include(x => x.OrderDetails)
              .Include(x => x.RequestPay)
              .FirstOrDefault();


            return order ?? new Order();
        }
    }
}
