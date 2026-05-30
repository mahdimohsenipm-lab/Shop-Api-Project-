using Entites.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entites.Products
{
    public class Category:BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        public bool IsDelete { get; set; }

        public int? ParentCategoryId { get; set; }

        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> SubCategory { get; set; }

        public virtual ICollection<Product> ProductsName { get; set; }

    }
}
