using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Controllers
{
    /// <summary>
    /// UsersController provides all actions acording to users
    /// </summary>
    [Route("api/users")]
    [ApiController]
    [Authorize]
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

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userVM">User-Model including UserName, Password and RoleId</param>
        /// <returns></returns>
        // POST: api/Users
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
    }
}
