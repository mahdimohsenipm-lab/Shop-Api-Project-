using Entites.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Products
{
    public class ProductImage:BaseEntity
    {
        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public bool IsDelete { get; set; }


        public string Src { get; set; }
    }
}
