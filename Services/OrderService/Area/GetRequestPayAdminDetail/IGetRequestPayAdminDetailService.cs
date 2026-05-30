using Services.ViewModel.Area.Model.Dto;

namespace Services.OrderService.Area.GetRequestPayAdminDetail
{
    public interface IGetRequestPayAdminDetailService
    {
        Task<RequestPayDetailDto> Execute(int requestPayId);
    }
}
