using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inocrea.CodaBox.ApiServer.Entities;

namespace Inocrea.CodaBox.ApiServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly InosysDBContext _context;

        public TransactionsController(InosysDBContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Transactions>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transactions>> GetTransactions(int id)
        {
            var transactions = await _context.Transactions.FindAsync(id);
            if (transactions == null) return NotFound();

            var cb = await _context.CompteBancaire.FindAsync(transactions.CompteBancaireId);
            transactions.CompteBancaire = cb;

            return transactions;
        }

        [HttpGet("{statementId}")]
        public async Task<List<Transactions>> GetTransactionsByStatement(int statementId)
        {
            var transactions = await _context.Transactions.Where(i=>i.StatementId==statementId).ToListAsync();
            return transactions;
        }

    }
}
