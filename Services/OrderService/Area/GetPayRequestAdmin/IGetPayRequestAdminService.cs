using Services.ViewModel.Area.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Services.OrderService.Area.GetPayRequestAdmin
{
    public interface IGetPayRequestAdminService
    {
        IPagedList<PayRquestDto> Execute(int? page);
    }

}
