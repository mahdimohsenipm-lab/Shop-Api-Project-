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

        public GetDiscountCodeService(IRepository<DiscountCode> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            
        }
        public IPagedList<DiscountCodeDto> Execute(int? page)
        {
            var now = DateTimeOffset.Now;
            var result = _repository.TableNoTracking.Where(x => x.IsActive == true)
                .ProjectTo<DiscountCodeDto>(_mapper.ConfigurationProvider)
                .ToPagedList(page??1,5);

            return result;
        }
    }

}
