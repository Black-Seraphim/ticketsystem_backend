using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    // Login-Model for API
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
