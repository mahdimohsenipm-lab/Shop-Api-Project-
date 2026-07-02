using AutoMapper;
using Data.Contracts;
using Entites.Products;
using Microsoft.EntityFrameworkCore;

namespace Services.DiscountCodeService.Area.AddDiscountCode
{
    public class AddDiscountCodeService : IAddDiscountCodeService
    {

        private readonly IRepository<DiscountCode> _repository;
        private readonly IMapper mapper;

        public AddDiscountCodeService(IRepository<DiscountCode> repository,IMapper mapper)
        {
            _repository = repository;
            this.mapper = mapper;
        }
        public async Task Execute(RequestAddDiscountCode requestAdd, CancellationToken cancellationToken)
        {
            if (requestAdd == null)
            {
                throw new Exception("مقادیر را درست ارسال کنید");
            }

            var result = mapper.Map<DiscountCode>(requestAdd);

            result.Code = result.Code.Trim().ToUpper();

            var codeExist = await _repository.TableNoTracking
                .AnyAsync(x => x.Code == result.Code, cancellationToken);

            if (codeExist)
            {
                throw new Exception("این کد تخفیف قبلاً ثبت شده است.");
            }

            await _repository.AddAsync(result, cancellationToken);
        }
    }
}
