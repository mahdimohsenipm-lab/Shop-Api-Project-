using AutoMapper;
using Data.Contracts;
using Entites.Orders;
using Services.ViewModel.Area.Model.Dto;
using X.PagedList;
using X.PagedList.Extensions;

namespace Services.OrderService.Area.GetPayRequestAdmin
{
    public class GetPayRequestAdminService : IGetPayRequestAdminService
    {
        private readonly IRepository<RequestPay> _requestPayRepository;
        private readonly IMapper _mapper;

        public GetPayRequestAdminService(IRepository<RequestPay> repository,IMapper mapper)
        {
            _requestPayRepository = repository;
            _mapper = mapper;
        }
        public IPagedList<PayRquestDto> Execute(int? page)
        {
            int numberPage= page ?? 1;
            int pageSize = 10;
            var resultRequestPay = _requestPayRepository.TableNoTracking
                .OrderByDescending(x=>x.PayDate).ToPagedList(numberPage,pageSize);

            var result= resultRequestPay.Select(x=> _mapper.Map<PayRquestDto>(x));

            return result;
            
        }
    }
}
