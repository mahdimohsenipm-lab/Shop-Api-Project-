using Microsoft.AspNetCore.Http.HttpResults;
using Services.ViewModel.Area.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductsServices.Querys.GetProductSite
{
    public interface IGetProductSiteService
    {
        Task<ResultProductSiteDto> Execute(CancellationToken cancellationToken);
    }
}
