using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Migrations
{
    public class DbSeedData
    {
        private TicketSystemDbContext _context;

        public DbSeedData(TicketSystemDbContext context)
        {
            _context = context;
        }

        public void EnsureSeedData()
        {
            _context.Database.EnsureCreated();

            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role() { Name = "Student" });
                _context.Roles.Add(new Role() { Name = "Tutor" });
                _context.SaveChanges();
            }

            if (!_context.Users.Any())
            {
                _context.Users.Add(new User()
                {
                    FirstName = "Alexander",
                    LastName = "Student",
                    PasswordHash = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    FirstName = "Michael",
                    LastName = "Student",
                    PasswordHash = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    FirstName = "Thomas",
                    LastName = "Student",
                    PasswordHash = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    FirstName = "Mirja",
                    LastName = "Tutor",
                    PasswordHash = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Tutor")
                });

                _context.Users.Add(new User()
                {
                    FirstName = "Janina",
                    LastName = "Tutor",
                    PasswordHash = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Tutor")
                });
             
                _context.SaveChanges();
            }

            if (!_context.Modules.Any())
            {
                _context.Modules.Add(new Module()
                {
                    Name = "Jodeln für Anfänger",
                    Responsible = _context.Users.First(u => u.FirstName == "Mirja")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Diskuswerfen",
                    Responsible = _context.Users.First(u => u.FirstName == "Janina")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Karate mit Obst",
                    Responsible = _context.Users.First(u => u.FirstName == "Mirja")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Hangover Basics",
                    Responsible = _context.Users.First(u => u.FirstName == "Janina")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Ernährungswissenschaften",
                    Responsible = _context.Users.First(u => u.FirstName == "Mirja")
                });

                _context.SaveChanges();
            }

            if (!_context.Documents.Any())
            {
                _context.Documents.Add(new Document()
                {
                    Name = "Mahlen nach Zahlen 1",
                    Link = "http://www.mahlennachzahlen.de",
                    Module = _context.Modules.First(m => m.Name == "Jodeln für Anfänger")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Mahlen nach Zahlen 2",
                    Link = "http://www.mahlennachzahlen.de",
                    Module = _context.Modules.First(m => m.Name == "Diskuswerfen")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Mahlen nach Zahlen 3",
                    Link = "http://www.mahlennachzahlen.de",
                    Module = _context.Modules.First(m => m.Name == "Karate mit Obst")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Mahlen nach Zahlen 4",
                    Link = "http://www.mahlennachzahlen.de",
                    Module = _context.Modules.First(m => m.Name == "Hangover Basics")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Mahlen nach Zahlen 5",
                    Link = "http://www.mahlennachzahlen.de",
                    Module = _context.Modules.First(m => m.Name == "Ernährungswissenschaften")
                });
                _context.SaveChanges();

            }

            if (!_context.Tickets.Any())
            {
                _context.Tickets.Add(new Ticket()
                {
                    CreatedBy = _context.Users.First(u => u.FirstName == "Mirja"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = true,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 4")
                });

                _context.Tickets.Add(new Ticket()
                {
                    CreatedBy = _context.Users.First(u => u.FirstName == "Alexander"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = true,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 3")
                });

                _context.Tickets.Add(new Ticket()
                {
                    CreatedBy = _context.Users.First(u => u.FirstName == "Janina"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = false,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 2")
                });

                _context.Tickets.Add(new Ticket()
                {
                    CreatedBy = _context.Users.First(u => u.FirstName == "Michael"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = false,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 1")
                });

                _context.SaveChanges();
            }
        }
    }
}
