using AutoMapper;
using Data.Contracts;
using Entites.Products;
using StoreTest.Areas.Admin.Controllers;

namespace Services.DiscountCodeService.Area.ActivationService
{
    public class ActivationService : IActivationService
    {
        private readonly IRepository<DiscountCode> _repository;
        private readonly IMapper _mapper;

        public ActivationService(IRepository<DiscountCode> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task Execute(ActivationRequest request,CancellationToken cancellationToken)
        {
            var discount = await _repository.GetByIdAsync(cancellationToken, request.Id);

            var result= _mapper.Map(request,discount);

            await _repository.UpdateAsync(result,cancellationToken);
        }
    }
   
}
