using Services.OrderService.Site.AddOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Site
{
    public class RequestAddPay
    {
        public string Address { get; set; }

        public string DiscountCode { get; set; }

        public List<RequestProductsOrder> Items { get; set; }
    }
}
