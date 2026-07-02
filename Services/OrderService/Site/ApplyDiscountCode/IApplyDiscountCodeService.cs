using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Site.ApplyDiscountCode
{
    public interface IApplyDiscountCodeService
    {
        Task<ApplyDiscountCodeResponse> Execute(ApplyDiscountCodeRequest request,CancellationToken cancellationToken);
        
    }
}
