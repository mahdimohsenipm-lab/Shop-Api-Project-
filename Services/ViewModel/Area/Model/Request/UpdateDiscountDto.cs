using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Area.Model.Request
{
    public class UpdateDiscountDto
    {
        public int Id { get; set; }

        public int? AmountDiscount { get; set; }

        public int? PercentageDiscount { get; set; }

        public DateTimeOffset EndTime { get; set; }
    }
}
