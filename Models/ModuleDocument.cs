using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class ModuleDocument
    {
        public int DocumentId { get; set; }
        public int ModuleId { get; set; }
        public Document Document { get; set; }
        public Module Module { get; set; }

    }
}
