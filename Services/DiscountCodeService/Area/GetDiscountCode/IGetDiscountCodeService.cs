using Services.ViewModel.Area.Model.Dto;
using X.PagedList;

namespace Services.DiscountCodeService.Area.GetDiscountCode
{
    public interface IGetDiscountCodeService
    {
        IPagedList<DiscountCodeDto> Execute(int? page);
    }

}
