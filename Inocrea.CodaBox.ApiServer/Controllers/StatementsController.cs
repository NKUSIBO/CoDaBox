using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Inocrea.CodaBox.ApiServer.Entities;

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
            if (statements == null) return NotFound();

            var transactions = _context.Transactions.Where(t => t.StatementId == id).ToList();
            var cb = await _context.CompteBancaire.FindAsync(statements.CompteBancaireId);
            statements.CompteBancaire = cb;
            foreach(var tr in transactions)
            {
                var cbt = await _context.CompteBancaire.FindAsync(tr.CompteBancaireId);
                tr.CompteBancaire = cbt;
            }

            statements.Transactions = transactions;

            return statements;
        }

    }
}
