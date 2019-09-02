using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Inocrea.CodaBox.ApiServer.Entities;

using Microsoft.AspNetCore.Authorization;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Inocrea.CodaBox.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatementsController : ControllerBase
    {
        private readonly InosysDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatementsController(InosysDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: api/Statements
        [HttpGet]
       
        //[EnableQuery()]
        public async Task<ActionResult<IEnumerable<Statements>>> GetStatements()
        {
            try
            {
                var userN = HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault()?.Value;
                var statementByUser =  _context.Set<Statements>().FromSql("dbo.sp_statsByUser @UserName = {0}", userN);
                return  new ActionResult<IEnumerable<Statements>>(statementByUser);

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
        [HttpGet("{Iban}/{datepickerStart}/{datepickerEnd}")]
        public IEnumerable<Transactions> GetTransactionsByDateIban(string Iban, DateTime? datepickerStart, DateTime? datepickerEnd)
        {
            try
            {


                var userN = HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault()?.Value;
                var transactionsByDate = _context.Set<Transactions>().FromSql("dbo.sp_transactionByDateIban @StartDate = {0},@EndDate = {1},@UserName = {2},@Iban = {3}", datepickerStart, datepickerEnd, userN, Iban);
                return transactionsByDate;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

    }
}
