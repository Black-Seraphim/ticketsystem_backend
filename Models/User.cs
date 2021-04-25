using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }

    public class CreateUserVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

}
