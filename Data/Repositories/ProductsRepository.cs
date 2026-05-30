using Data.Contracts;
using Entites.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductsRepository : Repository<Product>, IProductsRepository
    {
        private readonly IRepository<ProductFiture> productfitureRepository;
        private readonly IRepository<ProductImage> productImageRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductsRepository(ApplicationDbContext dbContext, IRepository<ProductFiture>
            productfitureRepository, IRepository<ProductImage> productImageRepository, ICategoryRepository categoryRepository)
            : base(dbContext)
        {
            this.productfitureRepository = productfitureRepository;
            this.productImageRepository = productImageRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task DeleteProducts(int id,CancellationToken cancellationToken)
        {
            var products =Table.Include(x => x.ProductFitures).Include(x => x.ProductImages).FirstOrDefault(x=>x.Id==id);
            if (products==null)
            {
                throw new Exception("محصولی یافت نشد");
            }
            // await productfitureRepository.DeleteRangeAsync(products.ProductFitures, cancellationToken);
            //await productImageRepository.DeleteRangeAsync(products.ProductImages,cancellationToken);
            products.IsDelete = true;

            await UpdateAsync(products, cancellationToken);


           //    await DeleteAsync(products, cancellationToken);
        }

    }
}
