using Services.ViewModel.Area.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductsServices.Commands.UpdateProduct
{
    public interface IUpdateProductService
    {
        Task Execute(int id,UpdateRequestDto dto, CancellationToken cancellationToken);
    }
}
