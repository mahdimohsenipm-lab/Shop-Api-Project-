using Data.Contracts;
using Entites.Products;
using Microsoft.EntityFrameworkCore;

namespace Services.OrderService.Site.ApplyDiscountCode
{
    public class ApplyDiscountCodeService: IApplyDiscountCodeService
    {
        private readonly IRepository<DiscountCode> _discountCoderepository;
        private readonly IProductsRepository _productsRepository;

        public ApplyDiscountCodeService(IRepository<DiscountCode> DiscountCoderepository, IProductsRepository ProductsRepository)
        {
            _discountCoderepository = DiscountCoderepository;
            _productsRepository=ProductsRepository;
        }
        public async Task<ApplyDiscountCodeResponse> Execute(ApplyDiscountCodeRequest request, CancellationToken cancellationToken)
        {
            var productsid = request.Items.Select(x=>x.ProductId).ToList();

            var products =await _productsRepository.TableNoTracking
             .Where(x => productsid.Contains(x.Id))
             .ToListAsync(cancellationToken);


            decimal totalPrice = 0;

            foreach (var item in request.Items)
            {
                var product = products.First(x => x.Id == item.ProductId);

                totalPrice += product.Price * item.Count;
            }

            DateTimeOffset now = DateTimeOffset.Now;
            var code = request.Code.ToUpper().Trim();
            var DiscountCode =await _discountCoderepository.TableNoTracking.FirstOrDefaultAsync(x=>x.IsActive==true
            &&x.Count>0&&x.StartTime<now&&x.EndTime>now&&totalPrice>x.LimitPrice&&x.Code==code);

            if (DiscountCode==null)
            {
                throw new Exception("کد تخفیف پیدا نشد");
            }

            decimal finalprice = CalculatFinalPrice(DiscountCode.Amount.Value, DiscountCode.Percentage.Value, totalPrice);
            ApplyDiscountCodeResponse result = new ApplyDiscountCodeResponse()
            {
                DiscountAmount = DiscountCode.Amount.Value,
                DiscountPercrntage = DiscountCode.Percentage.Value,
                FinalPrice = finalprice,
                TotalPrice=totalPrice


            };

            return result;
        }
        private decimal CalculatFinalPrice(decimal DiscountAmount,int Percentage,decimal totalPrice)
        {
            decimal finalPrice = 0;
            if (Percentage==0)
            {
                 finalPrice = totalPrice - DiscountAmount;
            }
            else
            {
                decimal discountAmount = totalPrice * Percentage / 100;
                finalPrice = totalPrice - discountAmount;
            }
            return finalPrice;
        }
    }
}
