using Entites.Common;

namespace Entites.Products
{
    public class ProductDiscount:BaseEntity
    {
        public int? Percentage { get; set; }

        public int? Amount  { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

      
        public bool IsActive { get; set; } 
    }
}
