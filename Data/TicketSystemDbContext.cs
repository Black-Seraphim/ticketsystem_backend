using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Data
{
    /// <summary>
    /// Database context, that is based on the database models
    /// </summary>
    public class TicketSystemDbContext : DbContext
    {
        public TicketSystemDbContext (DbContextOptions<TicketSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
