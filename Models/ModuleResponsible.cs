using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class ModuleResponsible
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public User User { get; set; }
        public Module Module { get; set; }

    }
}
