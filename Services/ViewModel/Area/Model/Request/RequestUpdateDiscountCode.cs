using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Area.Model.Request
{
    public class RequestUpdateDiscountCode
    {
        public int Id { get; set; }
        public int LimitPrice { get; set; }

        public int Amount { get; set; }

        public int Percentage { get; set; }

        public string Code { get; set; }

    }
}
