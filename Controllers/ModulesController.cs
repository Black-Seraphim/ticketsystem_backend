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
    /// ModulesController provides all actions acording to modules
    /// </summary>
    [Route("api/modules")]
    [ApiController]
    [Authorize]
    public class ModulesController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public ModulesController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all modules
        /// </summary>
        /// <returns></returns>
        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
            return await _context.Modules
                .ToListAsync();
        }

        /// <summary>
        /// Returns a module according to the send ModuleId
        /// </summary>
        /// <param name="id">moduleId</param>
        /// <returns></returns>
        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id)
        {
            var @module = await _context.Modules.Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (@module == null)
            {
                return NotFound();
            }

            return @module;
        }

        /// <summary>
        /// Creates a new module
        /// </summary>
        /// <param name="moduleVM">Module-Model including Name and ResponsibleUserId</param>
        /// <returns></returns>
        // POST: api/Modules
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(CreateModuleVM moduleVM)
        {
            // get responsible user
            User responsible = _context.Users.Where(u => u.Id == moduleVM.ResponsibleUserId).FirstOrDefault();

            // create new module
            Module module = new()
            {
                Name = moduleVM.Name,
                Responsible = responsible
            };

            // add module to database
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            // return new module
            return CreatedAtAction("GetModule", new { id = module.Id }, module);
        }
    }
}
