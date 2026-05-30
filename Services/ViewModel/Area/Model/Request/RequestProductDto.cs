using Services.ViewModel.Area.Model.Dto;
using X.PagedList;

namespace Services.ViewModel.Area.Model.Request
{
     public class RequestProductDto
    {
        public IPagedList<ProductDto> Products { get; set; }

    }

}
