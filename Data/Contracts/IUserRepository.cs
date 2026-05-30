using Data.Repositories;
using Entites.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IUserRepository :IRepository<User>
    {
          Task<User> GetByUserAndPass(string Name , string Password , CancellationToken cancellationToken);

          Task AddAsync(User user,string Password, CancellationToken cancellationToken);
          Task LastLoginDate(User user, CancellationToken requestAborted);

          Task UserSatusChange(User user, CancellationToken cancellationToken);
    }
}
