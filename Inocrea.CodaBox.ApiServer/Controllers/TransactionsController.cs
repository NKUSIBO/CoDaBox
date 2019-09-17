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
    [Route("api/[controller]/[action]")]
    public class TransactionsController : ControllerBase
    {
        private readonly DbInosalesContext _context;

        public TransactionsController(DbInosalesContext context)
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

            if (transactions == null)
            {
                return NotFound();
            }

            return transactions;
        }
        [HttpGet("{statementId}")]
        public async Task<List<Transactions>> GetTransactionsByStatement(int statementId)
        {
            try
            {
                var transactions = await _context.Transactions.Where(i => i.StatementId == statementId).Include(t => t.CompteBancaire).ToListAsync();
                return transactions;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet("{statementId}/{datepickerStart}/{datepickerEnd}")]
        public IEnumerable<Transactions> GetTransactionsByDate(int statementId, DateTime? datepickerStart, DateTime? datepickerEnd)
        {
            try
            {


                var userN = HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault()?.Value;
                var transactionsByDate = _context.Set<Transactions>().FromSql("dbo.sp_transactionByDate @StartDate = {0},@EndDate = {1},@UserName = {2},@StatementId = {3}", datepickerStart, datepickerEnd, userN, statementId);
                return transactionsByDate;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactions(int id, Transactions transactions)
        {
            if (id != transactions.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionsExists(id))
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

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transactions>> PostTransactions(Transactions transactions)
        {
            _context.Transactions.Add(transactions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactions", new { id = transactions.Id }, transactions);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transactions>> DeleteTransactions(int id)
        {
            var transactions = await _context.Transactions.FindAsync(id);
            if (transactions == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transactions);
            await _context.SaveChangesAsync();

            return transactions;
        }

        private bool TransactionsExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
