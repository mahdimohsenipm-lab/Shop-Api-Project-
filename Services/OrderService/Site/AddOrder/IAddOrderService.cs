using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Site.AddOrder
{
    public interface IAddOrderService
    {
        Task Execute(RequestAddOrder order, CancellationToken cancellationToken); 
    }
}
