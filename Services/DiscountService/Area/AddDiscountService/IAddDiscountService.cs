using static Services.DiscountService.Area.AddDiscountService.AddDiscountService;


namespace Services.DiscountService.Area.AddDiscountService
{
    public interface IAddDiscountService
    {
        Task Execute(CancellationToken cancellationToken, RequestDiscount requestDiscount);
    }
}
