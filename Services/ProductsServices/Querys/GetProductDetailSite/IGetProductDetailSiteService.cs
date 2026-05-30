using AutoMapper;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductsServices.Querys.GetProductDetailSite
{
    public interface IGetProductDetailSiteService
    {
        Task<ProductDetailDto> Execute(int id, CancellationToken cancellationToken);
    }
    public class GetProductDetailSiteService : IGetProductDetailSiteService
    {
        private readonly IProductsRepository productsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetProductDetailSiteService(IProductsRepository productsRepository,IMapper mapper,ICategoryRepository categoryRepository)
        {
            this.productsRepository = productsRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        public async Task<ProductDetailDto> Execute(int id,CancellationToken cancellationToken)
        {
            var product = await productsRepository.TableNoTracking
      .Include(x => x.ProductFitures)
      .Include(x => x.ProductImages)
      .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return null;

            var category = await categoryRepository.GetByIdAsync(cancellationToken, product.CategoriId);


            var mapDto = mapper.Map<ProductDetailDto>(product);

            mapDto.Category = category.Name;
            return mapDto;

        }
    }

    public class ProductDetailDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Category { get; set; }

        public int Inventory { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public List<ProductFitureDto> ProductFitures { get; set; }

        public List<ProductImageDto> ProductImages { get; set; }


    }
    public class ProductFitureDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }

    public class ProductImageDto
    {
        public int Id { get; set; }

        public string Src { get; set; }



    }
}
