using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Area.Model.Request
{
    public class RequestAddUser
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public string PasswordHash { get; set; }

        public int Age { get; set; }

        public bool IsActive { get; set; }

    }
}
