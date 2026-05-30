using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Services.ViewModel.Area.Model.Dto;

namespace Services.CategoryService.Site.GetCategoryForSite
{
    public class GetCategoryForSiteService : IGetCategoryForSiteService
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;

        public GetCategoryForSiteService(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoryDto>> Execute()
        {
            var result= await _categoryRepository.TableNoTracking
                 .Where(x => x.ParentCategoryId == null)
                 .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return result;

        }
    }
}
