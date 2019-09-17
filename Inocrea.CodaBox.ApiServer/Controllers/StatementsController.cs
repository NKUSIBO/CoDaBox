using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.ApiServer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Inocrea.CodaBox.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementsController : ControllerBase
    {
        private readonly DbInosalesContext _context;

        public StatementsController(DbInosalesContext context)
        {
            _context = context;
        }

        // GET: api/Statements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Statements>>> GetStatements()
        {
            try
            {
                var userN = HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault()?.Value;
                var statementByUser =  _context.Set<Statements>().FromSql("dbo.sp_statsByUser @UserName = {0}", userN);
                return new ActionResult<IEnumerable<Statements>>(statementByUser);

            }
            catch (System.Exception ex)
            {

                throw ex;
            }
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
            _context.Statements.Add(statements);
            await _context.SaveChangesAsync();

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
