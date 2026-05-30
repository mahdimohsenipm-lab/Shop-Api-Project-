using Data.Contracts;
using Entites.Orders;
using Services.OrderService.Site.AddOrder;

namespace Services.OrderService.Site.AddPayRequest
{
    public class AddRequestPay : IAddRequestPay
    {
        private readonly IRepository<RequestPay> _requestPayRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAddOrderService _addOrderService;

        public AddRequestPay(IRepository<RequestPay> repository, IUserRepository userRepository, IAddOrderService addOrderService)
        {
            _requestPayRepository = repository;
            _userRepository = userRepository;
            _addOrderService = addOrderService;
        }
        public async Task<RequestPay> Execute(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");
            var newRequestPay = new RequestPay
            {
                Guid = Guid.NewGuid(),
               
                UserId = user.Id,
                IsPay = false,
                PayDate = DateTime.UtcNow,
                Authority = null,
            };
            try
            {
                await _requestPayRepository.AddAsync(newRequestPay, cancellationToken); // saveNow=true پیش‌فرض

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return newRequestPay;
        }
    }
}
