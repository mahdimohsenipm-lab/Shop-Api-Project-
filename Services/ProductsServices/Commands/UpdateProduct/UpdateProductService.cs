using Data.Contracts;
using Services.ViewModel.Area.Model.Request;

namespace Services.ProductsServices.Commands.UpdateProduct
{
    public class UpdateProductService : IUpdateProductService
    {
        private readonly IProductsRepository productsRepository;
        public UpdateProductService(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;
            
        }
        public async Task Execute(int id, UpdateRequestDto dto,CancellationToken cancellationToken)
        {
            var Products=await productsRepository.GetByIdAsync(cancellationToken,id);

            Products.Name=dto.Name;

            Products.Price = dto.Price;

            Products.Inventory = dto.Inventory;

            await productsRepository.UpdateAsync(Products,cancellationToken);

        }
    }
}
