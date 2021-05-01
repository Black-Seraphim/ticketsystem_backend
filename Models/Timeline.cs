using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class Timeline
    {
        public string Month { get; set; }
        public int OpenedTickets { get; set; }
        public int ClosedTickets { get; set; }
    }

    public class TicketStat
    {
        public string ModulName { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
    }
}
