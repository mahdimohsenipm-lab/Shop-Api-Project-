using Services.ViewModel.Area.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Services.ViewModel.Area.Model.Request
{
    public class RequestGetDiscount
    {
        public IPagedList<DiscountDto> Discounts { get; set; }
    }
}
