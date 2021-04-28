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
            // Ensure database is created
            _context.Database.EnsureCreated();

            // Seed roles if no role exist
            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role() { Name = "Student" });
                _context.Roles.Add(new Role() { Name = "Tutor" });
                _context.SaveChanges();
            }

            // Seed users if no user exist
            if (!_context.Users.Any())
            {
                _context.Users.Add(new User()
                {
                    UserName = "Alexander.Kathan@iubh-fernstudium.de",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    UserName = "Michael.Ziaja@iubh-fernstudium.de",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    UserName = "Thomas.Hetfeld@iubh-fernstudium.de",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    UserName = "Mirja.Sirisko@iubh-fernstudium.de",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Tutor")
                });

                _context.Users.Add(new User()
                {
                    UserName = "Janina.Mantel@iubh-fernstudium.de",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Tutor")
                });
             
                _context.SaveChanges();
            }

            // Seed modules if no module exist
            if (!_context.Modules.Any())
            {
                _context.Modules.Add(new Module()
                {
                    Name = "Jodeln für Anfänger",
                    Responsible = _context.Users.First(u => u.UserName == "Mirja.Sirisko@iubh-fernstudium.de")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Diskuswerfen",
                    Responsible = _context.Users.First(u => u.UserName == "Janina.Mantel@iubh-fernstudium.de")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Karate mit Obst",
                    Responsible = _context.Users.First(u => u.UserName == "Mirja.Sirisko@iubh-fernstudium.de")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Hangover Basics",
                    Responsible = _context.Users.First(u => u.UserName == "Janina.Mantel@iubh-fernstudium.de")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "Ernährungswissenschaften",
                    Responsible = _context.Users.First(u => u.UserName == "Mirja.Sirisko@iubh-fernstudium.de")
                });

                _context.SaveChanges();
            }

            // Seed documents if no document exist
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

            // Seed tickets if no ticket exist
            if (!_context.Tickets.Any())
            {
                _context.Tickets.Add(new Ticket()
                {
                    Title = "Inhalt total unverständlich",
                    Description = "Bitte Abschnitt 3.2 neu formulieren",
                    CreatedBy = _context.Users.First(u => u.UserName == "Mirja.Sirisko@iubh-fernstudium.de"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = true,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 4"),
                    LastChangedBy = _context.Users.First(u => u.UserName == "Janina.Mantel@iubh-fernstudium.de"),
                    LastChangedDate = DateTime.Now
                });

                _context.Tickets.Add(new Ticket()
                {
                    Title = "Inhalt unverständlich",
                    Description = "Bitte Abschnitt 3.2 neu formulieren",
                    CreatedBy = _context.Users.First(u => u.UserName == "Alexander.Kathan@iubh-fernstudium.de"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = true,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 3")
                });

                _context.Tickets.Add(new Ticket()
                {
                    Title = "Inhalt unverständlich",
                    Description = "Bitte Abschnitt 3.2 neu formulieren",
                    CreatedBy = _context.Users.First(u => u.UserName == "Janina.Mantel@iubh-fernstudium.de"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = false,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 2")
                });

                _context.Tickets.Add(new Ticket()
                {
                    Title = "Inhalt unverständlich",
                    Description = "Bitte Abschnitt 3.2 neu formulieren",
                    CreatedBy = _context.Users.First(u => u.UserName == "Michael.Ziaja@iubh-fernstudium.de"),
                    CreatedDate = DateTime.Now,
                    TicketClosed = false,
                    Document = _context.Documents.First(d => d.Name == "Mahlen nach Zahlen 1")
                });

                _context.SaveChanges();
            }

            // Seed comments if no comment exist
            if (!_context.Comments.Any())
            {
                _context.Comments.Add(new Comment()
                {
                    CreatedBy = _context.Users.First(u => u.UserName == "Michael.Ziaja@iubh-fernstudium.de"),
                    CreatedDate = DateTime.Now,
                    Text = "Lern doch einfach lesen, oder zieh Dir ne Brille an!",
                    Ticket = _context.Tickets.First(t => t.Title == "Inhalt total unverständlich")
                });

                _context.SaveChanges();
            }
        }
    }
}
