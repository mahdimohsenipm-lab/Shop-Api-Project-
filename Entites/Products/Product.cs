using Entites.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entites.Products
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Brand { get; set; }
        [StringLength(500)]

        public string Description { get; set; }
        public int Inventory { get; set; }

        public bool Displayed { get; set; }

        public bool IsDelete { get; set; }

        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Category")]

        public int CategoriId { get; set; }


        public virtual ICollection<ProductFiture> ProductFitures { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }


    }
}
