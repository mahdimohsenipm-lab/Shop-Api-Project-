using Entites.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Jwt
{
    public interface IJwtServic
    {
        public Task<string> GenerateAsync(User user);

    }
}
