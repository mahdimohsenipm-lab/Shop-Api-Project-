using Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public  interface IProductsRepository:IRepository<Product>
    {
        
        
        Task DeleteProducts(int id, CancellationToken cancellationToken);

    }
}
