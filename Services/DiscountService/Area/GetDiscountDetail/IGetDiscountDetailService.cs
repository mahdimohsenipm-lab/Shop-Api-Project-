using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.DiscountService.Area.GetDiscountDetail
{
    public interface IGetDiscountDetailService
    {
        Task<ProductDiscountDetaliDto> Execute(int Id,bool? index, CancellationToken cancellationToken);
    }
}
