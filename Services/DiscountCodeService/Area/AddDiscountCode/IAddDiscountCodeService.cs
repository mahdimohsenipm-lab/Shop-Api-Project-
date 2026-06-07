using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DiscountCodeService.Area.AddDiscountCode
{
    public interface IAddDiscountCodeService
    {
        Task Execute(RequestAddDiscountCode requestAdd,CancellationToken cancellationToken);
    }
}
