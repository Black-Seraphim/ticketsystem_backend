using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class TicketComment
    {
        public int TicketId { get; set; }
        public int CommentId { get; set; }
        public Ticket Ticket { get; set; }
        public Comment Comment { get; set; }

    }
}
