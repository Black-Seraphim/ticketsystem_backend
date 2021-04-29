using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Ticket Ticket { get; set; }
        public string Text { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // Comment-Model for API
    public class CreateCommentVM
    {
        public int TicketID { get; set; }
        public string Text { get; set; }
    }
}
