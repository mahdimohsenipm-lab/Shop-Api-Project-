using Services.ViewModel.Area.Model.Dto;
using static Services.ProductsServices.Commands.AddProduct.AddProductService;

namespace Services.ProductsServices.Commands.AddProduct
{
    public interface IAddProductService
    {
        Task addProducts(RequestAddProducts Productdto, CancellationToken cancellationToken);
    }
}
