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
    [Route("api/[controller]")]
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
                .Include(t => t.Document.Module.Responsible)
                .Include(t => t.CreatedBy)
                .Include(t => t.LastChangedBy)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of all tickets related to a ModuleId
        /// </summary>
        /// <param name="id">ModuleId</param>
        /// <returns></returns>
        // GET: api/Ticktes/GetByModuleId/5
        [HttpGet("GetByModuleId/{id}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetModuleTickets(int id)
        {
            // Gets all documents that are included in the Course
            IEnumerable<Document> documents = _context.Documents.Where(d => d.Module.Id == id);

            // Gets all tickets that are assigned to the documents
            return await _context.Tickets.Where(t => documents.Contains(t.Document))
                .Include(t => t.Document.Module.Responsible)
                .Include(t => t.CreatedBy)
                .Include(t => t.LastChangedBy)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a list of all tickets assigned to the Tutor that is logged in
        /// </summary>
        /// <returns></returns>
        // GET: api/Tickets/TutorTickets
        [HttpGet("GetByTutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTutorTickets()
        {
            // Get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            string userRole = loggedUser.FindFirst(ClaimTypes.Role).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // Get modules where the tutor is responsible for
            IEnumerable<Module> modules = _context.Modules.Where(m => m.Responsible == user);

            // Get documents that are related to the modules
            IEnumerable<Document> documents = _context.Documents.Where(d => modules.Contains(d.Module));

            // Gets all tickets that are assigned to the documents
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
        // GET: api/Tickets/UserTickets
        [HttpGet("GetByUser")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetUserTickets()
        {
            // Get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // Get tickets created by the user
            return await _context.Tickets.Where(t => t.CreatedBy == user)
                .Include(t => t.Document.Module.Responsible)
                .Include(t => t.CreatedBy)
                .Include(t => t.LastChangedBy)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a ticket according to the send TicketId.
        /// Includes all Documents to the ticket.
        /// </summary>
        /// <param name="id">TicketId</param>
        /// <returns></returns>
        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketVM>> GetTicket(int id)
        {
            // Get ticket
            var ticket = await _context.Tickets.FindAsync(id);

            // Check if ticket exist
            if (ticket == null)
            {
                return NotFound();
            }

            // Get all documents related to the ticket
            List<Comment> comments = _context.Comments.Where(c => c.Ticket.Id == ticket.Id).ToList();

            // Create a new Ticket-Model that contains a list of related documents
            TicketVM ticketVM = new TicketVM()
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

        //// PUT: api/Tickets/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //[Authorize(Roles = "Tutor")]
        //public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        //{
        //    if (id != ticket.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(ticket).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TicketExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        /// <summary>
        /// Creates a new ticket
        /// </summary>
        /// <param name="ticketVM">Ticket-Model including Title, Description and DocumentId</param>
        /// <returns></returns>
        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(CreateTicketVM ticketVM)
        {
            // Get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // Get related document
            Document document = _context.Documents.Find(ticketVM.DocumentId);

            // Create new ticket
            Ticket ticket = new Ticket
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

            // Add ticket to database
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Return new ticket
            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        /// <summary>
        /// Change the TicketStatus from false to true, or vice versa
        /// </summary>
        /// <param name="id">TicketId</param>
        /// <returns></returns>
        // GET: api/Tickets/ChangeStatus/5
        [HttpPost("ChangeStatus/{id}")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<Ticket>>> ChangeTicketStatus(int id)
        {
            // Get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // Get ticket
            Ticket ticket = _context.Tickets.Find(id);

            // Check if ticket exist
            if (ticket == null)
            {
                return BadRequest();
            }

            // Change status
            ticket.TicketClosed = !ticket.TicketClosed;

            // Change LastChengedBy and LastChangedDate
            ticket.LastChangedBy = user;
            ticket.LastChangedDate = DateTime.Now;

            // Update Database
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        //// DELETE: api/Tickets/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTicket(int id)
        //{
        //    var ticket = await _context.Tickets.FindAsync(id);
        //    if (ticket == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Tickets.Remove(ticket);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        /// <summary>
        /// Returns true if ticket exist
        /// </summary>
        /// <param name="id">TicketId</param>
        /// <returns></returns>
        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
