using Entites.Orders;
using Entites.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Site.GetOrder
{
    public interface IGetOrderService
    {
       Order Execute(int requestPayId);
    }
}
