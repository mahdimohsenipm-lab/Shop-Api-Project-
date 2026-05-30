using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductsServices.Querys.GetProductDetail
{
    public interface IGetProductDetailService
    {
        Task<ResultDtoProduct> Execute(int id , CancellationToken cancellationToken);
        
    }
}
