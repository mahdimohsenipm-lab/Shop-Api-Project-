
using Entites.Products;

namespace Services.ViewModel.Area.Model.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public  List<Category> SubCategory { get; set; }

        public bool HasChildern { get; set; }
        public  Category ParentCategory { get; set; }
    }
}
