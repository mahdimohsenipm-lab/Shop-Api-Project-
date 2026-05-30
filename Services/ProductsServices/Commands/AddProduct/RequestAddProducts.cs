using Microsoft.AspNetCore.Http;

namespace Services.ProductsServices.Commands.AddProduct
{
    public class RequestAddProducts
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public string Brand { get; set; }

        public int Inventory { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public int CategoryId { get; set; }
        public List<ProductFitureRequest> Features { get; set; }

        public List<IFormFile> Image { get; set; }
    }
}
