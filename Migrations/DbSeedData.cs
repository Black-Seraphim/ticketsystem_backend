using System.Linq;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Migrations
{
    public class DbSeedData
    {
        private readonly TicketSystemDbContext _context;

        public DbSeedData(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Makes sure, that the database with each table and all relations is created.
        /// Also checks each table for existing data. If there are no data inside of the table, it will be filled with sample data for testing
        /// </summary>
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
                    UserName = "alexander.kathan",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    UserName = "michael.ziaja",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    UserName = "thomas.hetfeld",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Student")
                });

                _context.Users.Add(new User()
                {
                    UserName = "mirja.sirisko",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Tutor")
                });

                _context.Users.Add(new User()
                {
                    UserName = "janina.mantel",
                    Password = "sicher",
                    Role = _context.Roles.First(r => r.Name == "Tutor")
                });

                _context.Users.Add(new User()
                {
                    UserName = "tobias.brueckmann",
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
                    Name = "IMT01 - Mathematik Grundlagen I",
                    Responsible = _context.Users.First(u => u.UserName == "tobias.brueckmann")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "BWIR01-01 - Einführung in das wissenschaftliche Arbeiten",
                    Responsible = _context.Users.First(u => u.UserName == "tobias.brueckmann")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "BBWL01 - Betriebswirtswirtschaftslehre",
                    Responsible = _context.Users.First(u => u.UserName == "mirja.sirisko")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "BBWL02 - Betriebswirtswirtschaftslehre",
                    Responsible = _context.Users.First(u => u.UserName == "mirja.sirisko")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "IGIS01 - Grundlagen der industriellen Softwaretechnik",
                    Responsible = _context.Users.First(u => u.UserName == "mirja.sirisko")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "BCTR01-01 - Computer Training",
                    Responsible = _context.Users.First(u => u.UserName == "janina.mantel")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "IOBP01 - Objektorientierte Programmierung",
                    Responsible = _context.Users.First(u => u.UserName == "janina.mantel")
                });

                _context.Modules.Add(new Module()
                {
                    Name = "IOBP02 - Objektorientierte Programmierung",
                    Responsible = _context.Users.First(u => u.UserName == "janina.mantel")
                });

                _context.SaveChanges();
            }

            // Seed documents if no document exist
            if (!_context.Documents.Any())
            {
                _context.Documents.Add(new Document()
                {
                    Name = "Skript",
                    Link = "http://www.hetfeld.name/material/skript-imt01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IMT01 - Mathematik Grundlagen I")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Präsentationen",
                    Link = "http://www.hetfeld.name/material/skript-imt01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IMT01 - Mathematik Grundlagen I")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Vodcasts",
                    Link = "http://www.hetfeld.name/material/skript-imt01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IMT01 - Mathematik Grundlagen I")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Podcasts",
                    Link = "http://www.hetfeld.name/material/skript-imt01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IMT01 - Mathematik Grundlagen I")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Repetitorium",
                    Link = "http://www.hetfeld.name/material/skript-imt01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IMT01 - Mathematik Grundlagen I")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Aufgezeichnete Tutorien",
                    Link = "http://www.hetfeld.name/material/skript-imt01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IMT01 - Mathematik Grundlagen I")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Skript",
                    Link = "http://www.hetfeld.name/material/skript-bwir01-01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BWIR01-01 - Einführung in das wissenschaftliche Arbeiten")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Präsentationen",
                    Link = "http://www.hetfeld.name/material/skript-bwir01-01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BWIR01-01 - Einführung in das wissenschaftliche Arbeiten")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Vodcasts",
                    Link = "http://www.hetfeld.name/material/skript-bwir01-01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BWIR01-01 - Einführung in das wissenschaftliche Arbeiten")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Podcasts",
                    Link = "http://www.hetfeld.name/material/skript-bwir01-01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BWIR01-01 - Einführung in das wissenschaftliche Arbeiten")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Workbookaufgaben",
                    Link = "http://www.hetfeld.name/material/skript-bwir01-01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BWIR01-01 - Einführung in das wissenschaftliche Arbeiten")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Skript",
                    Link = "http://www.hetfeld.name/material/skript-bbwl01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL01 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Präsentationen",
                    Link = "http://www.hetfeld.name/material/skript-bbwl01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL01 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Vodcasts",
                    Link = "http://www.hetfeld.name/material/skript-bbwl01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL01 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Podcasts",
                    Link = "http://www.hetfeld.name/material/skript-bbwl01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL01 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Folien zu Videolektionen",
                    Link = "http://www.hetfeld.name/material/skript-bbwl01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL01 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Skript",
                    Link = "http://www.hetfeld.name/material/skript-bbwl02.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL02 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Präsentationen",
                    Link = "http://www.hetfeld.name/material/skript-bbwl02.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL02 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Vodcasts",
                    Link = "http://www.hetfeld.name/material/skript-bbwl02.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL02 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Podcasts",
                    Link = "http://www.hetfeld.name/material/skript-bbwl02.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL02 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Folien zu Videolektionen",
                    Link = "http://www.hetfeld.name/material/skript-bbwl02.pdf",
                    Module = _context.Modules.First(m => m.Name == "BBWL02 - Betriebswirtswirtschaftslehre")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Skript",
                    Link = "http://www.hetfeld.name/material/skript-igis01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IGIS01 - Grundlagen der industriellen Softwaretechnik")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Präsentationen",
                    Link = "http://www.hetfeld.name/material/skript-igis01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IGIS01 - Grundlagen der industriellen Softwaretechnik")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Vodcasts",
                    Link = "http://www.hetfeld.name/material/skript-igis01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IGIS01 - Grundlagen der industriellen Softwaretechnik")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Podcasts",
                    Link = "http://www.hetfeld.name/material/skript-igis01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IGIS01 - Grundlagen der industriellen Softwaretechnik")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Aufgabenblätter",
                    Link = "http://www.hetfeld.name/material/skript-igis01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IGIS01 - Grundlagen der industriellen Softwaretechnik")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Aufgezeichnete Tutorien",
                    Link = "http://www.hetfeld.name/material/skript-igis01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IGIS01 - Grundlagen der industriellen Softwaretechnik")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Videos",
                    Link = "http://www.hetfeld.name/material/skript-iobp01.pdf",
                    Module = _context.Modules.First(m => m.Name == "BCTR01-01 - Computer Training")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Skript",
                    Link = "http://www.hetfeld.name/material/skript-iobp01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP01 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Präsentationen",
                    Link = "http://www.hetfeld.name/material/skript-iobp01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP01 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Vodcasts",
                    Link = "http://www.hetfeld.name/material/skript-iobp01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP01 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Podcasts",
                    Link = "http://www.hetfeld.name/material/skript-iobp01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP01 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Aufgezeichnete Tutorien",
                    Link = "http://www.hetfeld.name/material/skript-iobp01.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP01 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Skript",
                    Link = "http://www.hetfeld.name/material/skript-iobp02.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP02 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Präsentationen",
                    Link = "http://www.hetfeld.name/material/skript-iobp02.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP02 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Vodcasts",
                    Link = "http://www.hetfeld.name/material/skript-iobp02.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP02 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Podcasts",
                    Link = "http://www.hetfeld.name/material/skript-iobp02.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP02 - Objektorientierte Programmierung")
                });

                _context.Documents.Add(new Document()
                {
                    Name = "Aufgezeichnete Tutorien",
                    Link = "http://www.hetfeld.name/material/skript-iobp02.pdf",
                    Module = _context.Modules.First(m => m.Name == "IOBP02 - Objektorientierte Programmierung")
                });

                _context.SaveChanges();
            }

            // Seed tickets if no ticket exist
            if (!_context.Tickets.Any())
            {
                // sample tickets will be created on live system
            }

            // Seed comments if no comment exist
            if (!_context.Comments.Any())
            {
                // sample comments will be created on live system
            }
        }
    }
}
