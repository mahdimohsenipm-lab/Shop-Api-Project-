using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Area.Model.Dto
{
    public class DiscountCodeDto
    {
        public int Count { get; set; }

        public int LimitPrice { get; set; }

        public int? Amount { get; set; }

        public int? Percentage { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

    }
}
