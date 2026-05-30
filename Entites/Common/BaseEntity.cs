using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Common
{
    public interface IEntity 
    {
    
    }
    public abstract class BaseEntity<T>: IEntity
    {
        public T Id { get; set; }


    }
    public abstract class BaseEntity : BaseEntity<int>
    {
        
    
    }
}
