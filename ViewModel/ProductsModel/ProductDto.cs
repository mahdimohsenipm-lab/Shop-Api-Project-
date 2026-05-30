
namespace StoreTest.Areas.Admin.ViewModel
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Price { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }
        

        public List<ProductFiture> ProductFiture { get; set; }

        public List<ProductImage> ProductImage { get; set; }


    }
}
