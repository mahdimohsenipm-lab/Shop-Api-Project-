using Services.ViewModel.Area.Model.Request;

namespace Services.DiscountService.Area.GetDiscountService
{
    public interface IGetDiscountService
    {
        RequestGetDiscount Execute(int? page);
    }
}
