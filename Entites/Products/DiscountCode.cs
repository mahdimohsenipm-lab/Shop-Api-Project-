using Entites.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Products
{
    public class DiscountCode:BaseEntity
    {
        public int Count { get; set; }

        public int? Amount { get; set; }

        public int? Percentage { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

    }
}
