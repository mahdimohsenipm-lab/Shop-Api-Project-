using Common.Exceptions;
using Data.Contracts;
using Entites.Orders;
using Entites.Users;
using Microsoft.AspNetCore.Identity;

namespace Services.OrderService.Site.AddOrder
{
    public class AddOrderService : IAddOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetails> _orderRepositoryDetail;

        private readonly UserManager<User> _userManager;
        private readonly IRepository<RequestPay> _requestPayRepository;
        private readonly IProductsRepository _productsRepository;

        public AddOrderService(IRepository<Order> orderRepository,UserManager<User> userManager,
            IRepository<RequestPay> requestPayRepository,IProductsRepository productsRepository,
            IRepository<OrderDetails> repository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _requestPayRepository = requestPayRepository;
            _productsRepository= productsRepository;
            _orderRepositoryDetail = repository;
        }

        public async Task Execute(RequestAddOrder order, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(order.UserId.ToString());
            if (user == null) return;
            var requestpay = await _requestPayRepository.GetByIdAsync(cancellationToken, order.PayRequestId);
            if (requestpay == null) return;

            var productIds = order.Items.Select(x => x.ProductId).ToList();
            var products = _productsRepository.Table.Where(x => productIds.Contains(x.Id)).ToList();

            var neworder = new Order
            {
                Address = order.Address,
                orderState = OrderState.Processing,
                UserId = user.Id,
                TotalPrice = 0,
                RequestPay=requestpay,
                RequestPayId=requestpay.Id
               
            };
            // ذخیره سفارش برای اینکه ID تولید شود
            await _orderRepository.AddAsync(neworder, cancellationToken);

            var neworderDetail = order.Items.Select(x =>
            {
                var product = products.FirstOrDefault(p => p.Id == x.ProductId);
                if (product == null) throw new BadRequestException("Product Not Found");
                return new OrderDetails
                {
                    OrderId = neworder.Id, // استفاده از آی‌دی تولید شده در مرحله قبل
                    ProductId = x.ProductId,
                    Count = x.Count,
                    Price = product.Price,
                    ProductBrand= product.Brand,
                    ProductName=product.Name,
                };
            }).ToList();

            neworder.TotalPrice  = neworderDetail.Sum(x => x.Price * x.Count);


            await _orderRepository.UpdateAsync(neworder, cancellationToken);

            await _orderRepositoryDetail.AddRangeAsync(neworderDetail, cancellationToken);
        


        }
    }
}
