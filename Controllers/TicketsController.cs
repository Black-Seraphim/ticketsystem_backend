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

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket()
        {
            return await _context.Tickets
                .Include(t => t.Document.Module.Responsible.Role)
                .Include(t => t.CreatedBy.Role)
                .Include(t => t.LastChangedBy.Role)
                .ToListAsync();
        }

        // GET: api/CourseTickets/5
        [HttpGet("GetByCourseId/{id}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetCourseTicket(int id)
        {
            IEnumerable<Document> documents = _context.Documents.Where(d => d.Module.Id == id);
            return await _context.Tickets.Where(t => documents.Contains(t.Document))
                .Include(t => t.Document)
                .Include(t => t.CreatedBy)
                .ToListAsync();
        }

        // GET: api/TutorTickets
        [HttpGet("GetByTutor")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTutorTicket()
        {
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).ToString();
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            IEnumerable<Module> modules = _context.Modules.Where(m => m.Responsible == user);
            IEnumerable<Document> documents = _context.Documents.Where(d => modules.Contains(d.Module));
            return await _context.Tickets.Where(t => documents.Contains(t.Document))
                .Include(t => t.Document)
                .Include(t => t.CreatedBy)
                .ToListAsync();
        }

        // GET: api/UserTickets
        [HttpGet("GetByUser")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetUserTicket()
        {
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).ToString();
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return await _context.Tickets.Where(t => t.CreatedBy == user)
                .Include(t => t.Document)
                .Include(t => t.CreatedBy)
                .ToListAsync();
        }

        
        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketVM>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            List<Comment> comments = _context.Comments.Where(c => c.Ticket.Id == ticket.Id).ToList();

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

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(CreateTicketVM ticketVM)
        {
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).ToString();
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            
            Document document = _context.Documents.Find(ticketVM.DocumentId);
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

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        // GET: api/UserTickets
        [HttpPost("ChangeStatus/{id}")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<Ticket>>> ChangeTicketStatus(int id)
        {
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).ToString();
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            Ticket ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return BadRequest();
            }

            ticket.TicketClosed = !ticket.TicketClosed;
            ticket.LastChangedBy = user;
            ticket.LastChangedDate = DateTime.Now;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
               
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
