using Entites.Products;

namespace Services.ProductsServices.Querys.GetProductDetail
{
    public class ResultDtoProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Category { get; set; }

        public int Inventory { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public List<ProductFiture> ProductFitures { get; set; }

        public List<ProductImage> ProductImages { get; set; }

    }
}
