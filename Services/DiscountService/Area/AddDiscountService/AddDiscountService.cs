using Data.Contracts;
using Entites.Products;

namespace Services.DiscountService.Area.AddDiscountService
{
    public partial class AddDiscountService : IAddDiscountService
    {
        private readonly IRepository<ProductDiscount> _repository;
        private readonly IProductsRepository _productsRepository;

        public AddDiscountService(IRepository<ProductDiscount> repository, 
            IProductsRepository productsRepository)
        {
            _repository=repository;
            _productsRepository=productsRepository;
        }
        public async Task Execute(CancellationToken cancellationToken, RequestDiscount requestDiscount)
        {
            var product =await _productsRepository.GetByIdAsync(cancellationToken,requestDiscount.ProductId);

            if (product == null) { throw new Exception("محصول یافت نشد"); }
          
            var result = new ProductDiscount 
            {
                ProductId = product.Id,
                Amount=requestDiscount.Amount??0,
                IsActive=true,
                EndTime=requestDiscount.EndTime,
                Percentage=requestDiscount.Percentage??0,
                StartTime= DateTimeOffset.UtcNow

            };

           await _repository.AddAsync(result,cancellationToken);
        }
    }
}
