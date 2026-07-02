using Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Site.GetPayRequest
{
    public interface IGetRequestServic
    {
        Task<RequestPay> Execute(Guid guid, CancellationToken cancellationToken);

    }
}
