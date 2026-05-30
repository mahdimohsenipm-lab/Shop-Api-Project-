using Entites.Common;
using Entites.Products;

namespace Entites.Orders
{
    public class OrderDetails : BaseEntity
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }

        public string ProductName { get; set; }

        public string ProductBrand { get; set; }

    }
    
}
