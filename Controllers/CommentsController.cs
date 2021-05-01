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
    public class CommentsController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public CommentsController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all existing comments
        /// </summary>
        /// <returns></returns>
        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments
                .Include(c => c.CreatedBy)
                .ToListAsync();
        }

        /// <summary>
        /// Returns all documents that are related to the send course number
        /// </summary>
        /// <param name="id">ModuleId</param>
        /// <returns></returns>
        // GET: api/Comments/GetByTicketId/5
        [HttpGet("GetByTicketId/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetTicketComments(int id)
        {
            // get ticket
            Ticket ticket = _context.Tickets
                .Where(t => t.Id == id)
                .Include(t => t.CreatedBy)
                .Include(t => t.Document.Module)
                .FirstOrDefault();

            // return all comments related to the ticket
            return await _context.Comments
                .Where(c => c.Ticket == ticket)
                .Include(c => c.CreatedBy)
                .ToListAsync();
        }

        /// <summary>
        /// Returns a comment matching the send id.
        /// </summary>
        /// <param name="id">id of the comment that should be returned</param>
        /// <returns></returns>
        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments
                .Where(c => c.Id == id)
                .Include(c => c.CreatedBy)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        //// PUT: api/Comments/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutComment(int id, Comment comment)
        //{
        //    if (id != comment.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(comment).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CommentExists(id))
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
        /// Creates a new Comment. Returns the created comment
        /// </summary>
        /// <param name="commentVM">Model including TicketId and Text</param>
        /// <returns></returns>
        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(CreateCommentVM commentVM)
        {
            // get registered user
            ClaimsPrincipal loggedUser = HttpContext.User;
            string userName = loggedUser.FindFirst(ClaimTypes.Name).Value;
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            // get related ticket
            Ticket ticket = _context.Tickets.Where(t => t.Id == commentVM.TicketID).FirstOrDefault();

            // create new comment
            Comment comment = new Comment
            {
                CreatedBy = user,
                CreatedDate = DateTime.Now,
                Text = commentVM.Text,
                Ticket = ticket
            };

            // add comment to database
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // return created comment
            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        //// DELETE: api/Comments/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteComment(int id)
        //{
        //    var comment = await _context.Comments.FindAsync(id);
        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Comments.Remove(comment);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        /// <summary>
        /// Returns true if comment with send ID exist
        /// </summary>
        /// <param name="id">CommentId</param>
        /// <returns></returns>
        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
