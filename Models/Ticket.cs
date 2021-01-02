using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Document Document { get; set; }
        public TicketState TicketState { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public User LastChangedBy { get; set; }
        public DateTime LastChangedDate { get; set; }

    }
}
