using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public UsersController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all users
        /// </summary>
        /// <returns></returns>
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserVM>>> GetUsers()
        {
            return await _context.Users.Select(u => new GetUserVM
            {
                Id = u.Id,
                Role = u.Role,
                UserName = u.UserName
            }).ToListAsync();
        }

        /// <summary>
        /// Returns a user according to the send userId
        /// </summary>
        /// <param name="id">userId</param>
        /// <returns></returns>
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserVM>> GetUser(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return new GetUserVM
            {
                Id = user.Id,
                Role = user.Role,
                UserName = user.UserName
            };
        }

        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
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
        /// Creates a new user
        /// </summary>
        /// <param name="userVM">User-Model including UserName, Password and RoleId</param>
        /// <returns></returns>
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUserVM userVM)
        {
            // get role
            Role role = _context.Roles.Where(r => r.Id == userVM.RoleId).FirstOrDefault();

            // create new user
            User user = new()
            {
                Password = userVM.Password,
                Role = role,
                UserName = userVM.UserName
            };

            // add user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // return user
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        /// <summary>
        /// Returns true if user exist
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns></returns>
        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
