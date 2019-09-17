//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Inocrea.CodaBox.ApiServer.Entities;

//using Microsoft.AspNet.OData;
//using Microsoft.AspNetCore.Authorization;
//using Inocrea.CodaBox.ApiModel.Models;
//using Inocrea.CodaBox.ApiModel.ViewModel;

//namespace Inocrea.CodaBox.ApiServer.Controllers
//{
//    [Route("api/[controller]/[action]")]
//    [ApiController]
//    [Authorize]
//    public class TransactionsController : ControllerBase
//    {
//        private readonly InosysDBContext _context;

//        public TransactionsController(InosysDBContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Transactions
//        [HttpGet]
        
//        public async Task<ActionResult<IEnumerable<Transactions>>> GetTransactions()
//        {
//            return await _context.Transactions.ToListAsync();
//        }

//        // GET: api/Transactions/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Transactions>> GetTransactions(int id)
//        {
//            var transactions = await _context.Transactions.FindAsync(id);
//            if (transactions == null) return NotFound();

//            var cb = await _context.CompteBancaire.FindAsync(transactions.CompteBancaireId);
//            transactions.CompteBancaire = cb;

//            return transactions;
//        }

//        [HttpGet("{statementId}")]
//        public async Task<List<Transactions>> GetTransactionsByStatement(int statementId)
//        {
//            try
//            {
//                var transactions = await _context.Transactions.Where(i => i.StatementId == statementId).Include(t => t.CompteBancaire).ToListAsync();
//                return transactions;
//            }
//            catch (System.Exception ex)
//            {

//                throw ex;
//            }
//        }
//        [HttpGet("{statementId}/{datepickerStart}/{datepickerEnd}")]
//        public  IEnumerable<Transactions> GetTransactionsByDate(int statementId,  DateTime? datepickerStart,  DateTime? datepickerEnd)
//        {
//            try
//            {

                
//                var userN = HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault()?.Value;
//                var transactionsByDate =  _context.Set<Transactions>().FromSql("dbo.sp_transactionByDate @StartDate = {0},@EndDate = {1},@UserName = {2},@StatementId = {3}", datepickerStart, datepickerEnd, userN, statementId);
//                return transactionsByDate;
//            }
//            catch (System.Exception ex)
//            {

//                throw ex;
//            }
//        }
//    }
//}
