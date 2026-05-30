using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Area.Model.Dto
{
    public class DiscountDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public int? Amount { get; set; }

        public int? Percentage { get; set; }

        public bool IsActive { get; set; }
    }
}
