using Entites.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Products
{
    public class ProductFiture:BaseEntity
    {
       
        public string Title { get; set; }

        public bool IsDelete { get; set; }

        public string Description { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

    }
}
