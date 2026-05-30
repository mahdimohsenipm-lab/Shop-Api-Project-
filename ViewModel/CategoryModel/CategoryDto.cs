using Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CategoryModel
{
    public class CategoryDto
    {
        public string Name { get; set; }

        public Category ParentCategory { get; set; }
    }
}
