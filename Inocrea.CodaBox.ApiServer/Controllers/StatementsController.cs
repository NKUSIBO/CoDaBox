using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inocrea.CodaBox.ApiServer.Entities;
using Microsoft.AspNet.OData;

namespace Inocrea.CodaBox.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementsController : ControllerBase
    {
        private readonly InosysDBContext _context;

        public StatementsController(InosysDBContext context)
        {
            _context = context;
        }

        // GET: api/Statements
        [HttpGet]
        //[EnableQuery()]
        public async Task<ActionResult<IEnumerable<Statements>>> GetStatements()
        {
            return await _context.Statements.ToListAsync();
        }

        // GET: api/Statements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Statements>> GetStatements(int id)
        {
            var statements = await _context.Statements.FindAsync(id);

            if (statements == null)
            {
                return NotFound();
            }

            return statements;
        }

        // PUT: api/Statements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatements(int id, Statements statements)
        {
            if (id != statements.StatementId)
            {
                return BadRequest();
            }

            _context.Entry(statements).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatementsExists(id))
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

        // POST: api/Statements
        [HttpPost]
        public async Task<ActionResult<Statements>> PostStatements(Statements statements)
        {
            try
            {
                _context.Statements.Add(statements);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return CreatedAtAction("GetStatements", new { id = statements.StatementId }, statements);
        }

        // DELETE: api/Statements/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Statements>> DeleteStatements(int id)
        {
            var statements = await _context.Statements.FindAsync(id);
            if (statements == null)
            {
                return NotFound();
            }

            _context.Statements.Remove(statements);
            await _context.SaveChangesAsync();

            return statements;
        }

        private bool StatementsExists(int id)
        {
            return _context.Statements.Any(e => e.StatementId == id);
        }
    }
}
