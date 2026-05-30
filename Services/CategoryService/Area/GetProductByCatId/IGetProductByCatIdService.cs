using Services.ProductsServices.Querys.GetProductSite;
using Services.ViewModel.Area.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CategoryService.Area.GetProductByCatId
{
    public interface IGetProductByCatIdService
    {
        Task<List<ProductSiteDto>> Execute(int catId);        
    }
}
