using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public TicketsController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all tickets
        /// </summary>
        /// <returns></returns>
        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.Document.Module)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of all tickets related to a moduleId
        /// </summary>
        /// <param name="id">moduleId</param>
        /// <returns></returns>
        // GET: api/Ticktes/GetByModuleId/5
        [HttpGet("GetByModuleId/{id}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetByModuleId(int id)
        {
            // get all documents that are included in the module
            IEnumerable<Document> documents = _context.Documents.Where(d => d.Module.Id == id);

            // get all tickets that are assigned to the documents
            return await _context.Tickets.Where(t => documents.Contains(t.Document))
                .ToListAsync();
        }

        /// <summary>
        /// Returns a summary of opened and closed tickets per month
        /// </summary>
        /// <returns></returns>
        // GET: api/Ticktes/TicketTimeline
        [HttpGet("TicketTimeline")]
        public async Task<ActionResult<IEnumerable<Timeline>>> GetTicketTimeline()
        {
            IEnumerable<Ticket> tickets = await _context.Tickets.ToListAsync();
            List<Timeline> timelines = new List<Timeline>();

            for (int i = 1; i < 13; i++)
            {
                DateTime date = new DateTime(1900, i, 1);
                int openedTickets = tickets.Where(t => t.CreatedDate.Month == i).Count();
                int closedTickets = tickets.Where(t => t.LastChangedDate.Month == i && t.TicketClosed == true).Count();

                Timeline timeline = new()
                {
                    Month = date.ToString("MMMM"),
                    OpenedTickets = openedTickets,
                    ClosedTickets = closedTickets
                };
                timelines.Add(timeline);
            }
                    
            return timelines;
        }

        /// <summary>
        /// Returns a summary of opened and closed tickets per module
        /// </summary>
        /// <returns></returns>
        // GET: api/Ticktes/TicketsPerModule
        [HttpGet("TicketsPerModule")]
        public async Task<ActionResult<IEnumerable<TicketStat>>> GetTicketsPerModule()
        {
            IEnumerable<Module> modules = await _context.Modules.ToListAsync();
            IEnumerable<Ticket> tickets = await _context.Tickets.ToListAsync();
            IEnumerable<Document> documents = await _context.Documents.ToListAsync();
            List<TicketStat> ticketStats = new List<TicketStat>();

            foreach (Module module in modules)
            {
                List<Document> moduleDocuments = documents.Where(d => d.Module == module).ToList();
                List<Ticket> moduleTickes = tickets.Where(t => moduleDocuments.Contains(t.Document)).ToList();
                TicketStat ticketStat = new()
                {
                    ModulName = module.Name,
                    OpenTickets = moduleTickes.Where(t => t.TicketClosed == true).Count(),
                    ClosedTickets = moduleTickes.Where(t => t.TicketClosed == false).Count()
                };
                ticketStats.Add(ticketStat);
            }

            return ticketStats;
        }

        /// <summary>
        /// Returns a list of tickets that contains the search string
        /// </summary>
        /// <param name="id">searchString</param>
        /// <returns></returns>
        // GET: api/Ticktes/SearchByTitle/searchstring
        [HttpGet("SearchByTitle/{searchString}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> SearchByTitle(string searchString)
        {
            return await _context.Tickets
                .Where(t => t.Title.Contains(searchString))
                .Include(t => t.CreatedBy)
                .Include(t => t.Document.Module)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of all tickets assigned to the tutor that is logged in
        /// </summary>
        /// <returns></returns>
        // GET: api/Tickets/GetByTutor
        [HttpGet("GetByTutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetByTutor()
        {
            // get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            string userRole = loggedUser.FindFirst(ClaimTypes.Role).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // get modules where the tutor is responsible for
            IEnumerable<Module> modules = _context.Modules.Where(m => m.Responsible == user);

            // get documents that are related to the modules
            IEnumerable<Document> documents = _context.Documents.Where(d => modules.Contains(d.Module));

            // gets all tickets that are assigned to the documents
            return await _context.Tickets.Where(t => documents.Contains(t.Document))
                .Include(t => t.Document.Module.Responsible)
                .Include(t => t.CreatedBy)
                .Include(t => t.LastChangedBy)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of tickets that are created by the registered user
        /// </summary>
        /// <returns></returns>
        // GET: api/Tickets/GetByUser
        [HttpGet("GetByUser")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetByUser()
        {
            // get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // get tickets created by the user
            return await _context.Tickets.Where(t => t.CreatedBy == user)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a ticket according to the send ticketId.
        /// Includes all Documents to the ticket.
        /// </summary>
        /// <param name="id">ticketId</param>
        /// <returns></returns>
        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketVM>> GetTicket(int id)
        {
            // get ticket
            Ticket ticket = await _context.Tickets
                .Where(t => t.Id == id)
                .Include(t => t.Document.Module.Responsible)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync();

            // check if ticket exist
            if (ticket == null)
            {
                return NotFound();
            }

            // get all documents related to the ticket
            List<Comment> comments = _context.Comments.Where(c => c.Ticket.Id == ticket.Id).ToList();

            // create a new Ticket-Model that contains a list of related documents
            TicketVM ticketVM = new()
            {
                Id = ticket.Id,
                CreatedBy = ticket.CreatedBy,
                CreatedDate = ticket.CreatedDate,
                Description = ticket.Description,
                Document = ticket.Document,
                LastChangedBy = ticket.LastChangedBy,
                LastChangedDate = ticket.LastChangedDate,
                TicketClosed = ticket.TicketClosed,
                Title = ticket.Title,
                Comments = comments
            };

            return ticketVM;
        }

        /// <summary>
        /// Creates a new ticket
        /// </summary>
        /// <param name="ticketVM">Ticket-Model including Title, Description and DocumentId</param>
        /// <returns></returns>
        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(CreateTicketVM ticketVM)
        {
            // get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // get related document
            Document document = _context.Documents.Find(ticketVM.DocumentId);

            // create new ticket
            Ticket ticket = new()
            {
                CreatedBy = user,
                CreatedDate = DateTime.Now,
                Description = ticketVM.Description,
                Document = document,
                LastChangedBy = user,
                LastChangedDate = DateTime.Now,
                TicketClosed = false,
                Title = ticketVM.Title
            };

            // add ticket to database
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // return new ticket
            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        /// <summary>
        /// Switch the ticket status
        /// </summary>
        /// <param name="id">ticketId</param>
        /// <returns></returns>
        // GET: api/Tickets/ChangeStatus/5
        [HttpPost("ChangeStatus/{id}")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<Ticket>>> PostChangeStatus(int id)
        {
            // get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // get ticket
            Ticket ticket = _context.Tickets.Find(id);

            // check if ticket exist
            if (ticket == null)
            {
                return BadRequest();
            }

            // change status
            ticket.TicketClosed = !ticket.TicketClosed;

            // change LastChangedBy and LastChangedDate
            ticket.LastChangedBy = user;
            ticket.LastChangedDate = DateTime.Now;

            // update database
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }
    }
}
