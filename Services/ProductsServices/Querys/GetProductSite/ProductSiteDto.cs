namespace Services.ProductsServices.Querys.GetProductSite
{
    public class ProductSiteDto
    {
        public int id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Src { get; set; }

        public int ProductDiscount { get; set; }

        public int FinalPrice { get; set; }
    }
}
