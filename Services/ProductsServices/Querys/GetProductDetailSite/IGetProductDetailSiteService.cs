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

        public bool IsDiscount { get; set; }

        public int Amount { get; set; }

        public int Percentage { get; set; }

        public int FinalPrice { get; set; }

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
