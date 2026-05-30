using Services.ViewModel.Area.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CategoryService.Site.GetCategoryForSite
{
    public interface IGetCategoryForSiteService
    {
        Task<List<CategoryDto>> Execute();
    }
}
