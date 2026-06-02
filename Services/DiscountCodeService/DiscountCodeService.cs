using AutoMapper;
using Data.Contracts;
using Entites.Products;

namespace Services.DiscountCodeService
{
    public class DiscountCodeService : IDiscountCodeService
    {

        private readonly IRepository<DiscountCode> _repository;
        private readonly IMapper mapper;

        public DiscountCodeService(IRepository<DiscountCode> repository,IMapper mapper)
        {
            _repository = repository;
            this.mapper = mapper;
        }
        public async Task Execute(RequestAddDiscountCode requestAdd,CancellationToken cancellationToken)
        {
            if (requestAdd==null)
            {
                throw new Exception("مقادیر را درست ارسال کنید");
            }
            var result=mapper.Map<DiscountCode>(requestAdd);

            await _repository.AddAsync(result ,cancellationToken);
          
        }
    }
}
