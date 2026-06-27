using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Entites.Products;
using Services.ViewModel.Area.Model.Dto;
using X.PagedList;
using X.PagedList.Extensions;

namespace Services.DiscountCodeService.Area.GetDiscountCode
{
    public class GetDiscountCodeService : IGetDiscountCodeService
    {
        private readonly IRepository<DiscountCode> _repository;

        private readonly IMapper _mapper;

        private readonly IGetTimeLineService _timeLineService;

        public GetDiscountCodeService(IRepository<DiscountCode> repository, IMapper mapper, IGetTimeLineService timeLineService)
        {
            _repository = repository;
            _mapper = mapper;
            _timeLineService = timeLineService;
        }
        public IPagedList<DiscountCodeDto> Execute(int? page)
        {
       
            var result = _repository.TableNoTracking
                .ProjectTo<DiscountCodeDto>(_mapper.ConfigurationProvider)
                .ToPagedList(page??1,5);

            foreach (var item in result)
            {
                var state = _timeLineService.Execute(item);

                //Apply in dto
                item.Apply(state);
                
            }
            return result;


        }
    }

}
