using System;
using System.Collections.Generic;

namespace ticketsystem_backend.Models
{
    // ticket model for ticket database
    public class Ticket
    {
        public int Id { get; set; }
        public Document Document { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool TicketClosed { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public User LastChangedBy { get; set; }
        public DateTime LastChangedDate { get; set; }
    }

    // Ticket-Model for API, that includes a list of according comments
    public class TicketVM
    {
        public int Id { get; set; }
        public Document Document { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool TicketClosed { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public User LastChangedBy { get; set; }
        public DateTime LastChangedDate { get; set; }
        public List<Comment> Comments { get; set; }
    }

    // Ticket-Model for API
    public class CreateTicketVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DocumentId { get; set; }
    }
}
