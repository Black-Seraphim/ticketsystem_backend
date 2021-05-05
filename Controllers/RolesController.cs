using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Controllers
{
    /// <summary>
    /// RolesController provides all actions acording to roles
    /// </summary>
    [Route("api/roles")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public RolesController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of all roles
        /// </summary>
        /// <returns></returns>
        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        /// <summary>
        /// Returns a role according to the send roleId
        /// </summary>
        /// <param name="id">roleId</param>
        /// <returns></returns>
        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        /// <summary>
        /// Create new role
        /// </summary>
        /// <param name="role">Role-Model including Name</param>
        /// <returns></returns>
        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            // add new role to database
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            // return new role
            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }
    }
}
