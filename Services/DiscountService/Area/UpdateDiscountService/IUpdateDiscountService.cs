using Data.Contracts;
using Entites.Products;
using Services.ViewModel.Area.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DiscountService.Area.UpdateDiscountService
{
    public interface IUpdateDiscountService
    {
        Task Execute(UpdateDiscountDto discountDto,CancellationToken cancellationToken);
    }
    public class UpdateDiscountService : IUpdateDiscountService
    {
        private readonly IRepository<ProductDiscount> _repository;

        public UpdateDiscountService(IRepository<ProductDiscount> repository)
        {
            _repository = repository;
        }
        public async Task Execute(UpdateDiscountDto discountDto, CancellationToken cancellationToken)
        {
            var discount = await _repository.GetByIdAsync(cancellationToken, discountDto.Id);
            if (discount == null)
            {
                throw new Exception("تخفیف یافت نشد");
            }
            if (discountDto.AmountDiscount == 0)
            {
                discount.Percentage = discountDto.PercentageDiscount;
                discount.EndTime = discountDto.EndTime;
                discount.Amount = 0;

            }
            else
            {
                discount.Percentage = 0;
                discount.Amount = discountDto.AmountDiscount;
                discount.EndTime = discountDto.EndTime;

            }
            await _repository.UpdateAsync(discount, cancellationToken);

        }
    }
}
