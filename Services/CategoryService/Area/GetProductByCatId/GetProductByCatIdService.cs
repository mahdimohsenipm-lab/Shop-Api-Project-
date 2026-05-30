using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Services.ProductsServices.Querys.GetProductSite;
using Services.ViewModel.Area.Model.Dto;

namespace Services.CategoryService.Area.GetProductByCatId
{
    public class GetProductByCatIdService : IGetProductByCatIdService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;
        public GetProductByCatIdService(IProductsRepository productsRepository ,IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductSiteDto>> Execute(int catId)
        {
            var products =await _productsRepository.TableNoTracking.Include(x=>x.ProductImages)
                .Where(x => x.CategoriId == catId).ProjectTo<ProductSiteDto>(_mapper.ConfigurationProvider).Take(6)
                .ToListAsync();

            return products;
        }


    }
}
