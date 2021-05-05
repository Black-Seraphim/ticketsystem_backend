namespace ticketsystem_backend.Models
{
    // timeline model for API to create statistics view
    public class Timeline
    {
        public string Month { get; set; }
        public int OpenedTickets { get; set; }
        public int ClosedTickets { get; set; }
    }

    // ticketstat model for API to create statistics view
    public class TicketStat
    {
        public string ModulName { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
    }
}
