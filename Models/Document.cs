using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public DocumentType DocumentType { get; set; }

    }
}
