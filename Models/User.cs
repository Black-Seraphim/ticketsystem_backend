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

    // User-Model for API
    public class CreateUserVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    // User-Model for API (GET)
    public class GetUserVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}
