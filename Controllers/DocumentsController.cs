using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DocumentsController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public DocumentsController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all documents
        /// </summary>
        /// <returns></returns>
        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            return await _context.Documents
                .ToListAsync();
        }

        /// <summary>
        /// Returns the document according to the send id
        /// </summary>
        /// <param name="id">documentId</param>
        /// <returns></returns>
        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            var document = await _context.Documents.Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

        /// <summary>
        /// Returns all documents that are related to the send course number
        /// </summary>
        /// <param name="id">moduleId</param>
        /// <returns></returns>
        // GET: api/Documents/GetByModuleId/5
        [HttpGet("GetByModuleId/{id}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetModuleDocuments(int id)
        {
            return await _context.Documents.Where(d => d.Module.Id == id)
                .ToListAsync();
        }

        //// PUT: api/Documents/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDocument(int id, Document document)
        //{
        //    if (id != document.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(document).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DocumentExists(id))
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
        /// Create a new document and returns it
        /// </summary>
        /// <param name="documentVM">Document-Model including Name, Link and ModuleId</param>
        /// <returns></returns>
        // POST: api/Documents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Document>> PostDocument(CreateDocumentVM documentVM)
        {
            // get related module
            Module module = _context.Modules.Where(m => m.Id == documentVM.ModuleId).FirstOrDefault();

            // create new document
            Document document = new Document
            {
                Link = documentVM.Link,
                Module = module,
                Name = documentVM.Name
            };

            // add document to database
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            // return new document
            return CreatedAtAction("GetDocument", new { id = document.Id }, document);
        }

        //// DELETE: api/Documents/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteDocument(int id)
        //{
        //    var document = await _context.Documents.FindAsync(id);
        //    if (document == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Documents.Remove(document);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        /// <summary>
        /// Returns true if Document exist
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <returns></returns>
        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
