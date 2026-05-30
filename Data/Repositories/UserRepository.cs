using Common.Utilities;
using Data.Contracts;
using Entites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly UserManager<User> userManager;

        public UserRepository(ApplicationDbContext dbContext,UserManager<User> userManager)
            : base(dbContext)
        {
            this.userManager=userManager;
        }
        public async Task AddAsync(User user, string Password, CancellationToken cancellationToken)
        {
            var exist = await TableNoTracking.AnyAsync(x => x.UserName == user.UserName);

            if (exist)
                throw new Exception("The Name Has Exists");
            var PasswordHash = SecurityHelper.GetSha256Hash(Password);

            user.PasswordHash = PasswordHash;
            await base.AddAsync(user, cancellationToken);



        }

        public Task<User> GetByUserAndPass(string Name, string Password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(Password);

            var user = Table.Where(x => x.UserName == Name && x.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);

            return user;
        }

        public Task LastLoginDate(User user, CancellationToken cancellationToken)
        {
            user.LoginTime = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }

        public Task UserSatusChange(User user, CancellationToken cancellationToken)
        {
            user.IsActive = !user.IsActive;
            return UpdateAsync(user, cancellationToken);

        }
     
    }
}
