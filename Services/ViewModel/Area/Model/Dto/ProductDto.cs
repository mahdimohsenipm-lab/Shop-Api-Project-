using Entites.Products;
using Microsoft.AspNetCore.Http;
using Services.ProductsServices.Commands.AddProduct;

namespace Services.ViewModel.Area.Model.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Price { get; set; }

        public string Brand { get; set; }

        public string Src { get; set; }

        public int Inventory { get; set; }

        public string Description { get; set; }

        public bool Displayed { get; set; }

        public int CategoryId { get; set; }
        public List<ProductFitureRequest> ProductFitureDto { get; set; }

        public List<IFormFile> Image { get; set; }

        public bool IsDiscount { get; set; }


       

    }
}
