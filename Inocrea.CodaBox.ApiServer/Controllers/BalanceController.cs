using System;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiServer.Entities;
using Inocrea.CodaBox.ApiServer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Inocrea.CodaBox.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly InosysDBContext _context;

        public BalanceController(InosysDBContext context)
        {
            _context = context;
        }

        // GET: api/Balance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BalanceViewModel>> GetCompteBalance(int id)
        {
            try
            {
                var compteBancaire = await _context.CompteBancaire.FindAsync(id);
                if (compteBancaire == null) { return NotFound(); }
                var statementID = _context.Statements.Where(s => s.CompteBancaireId == compteBancaire.Id).Max(s => s.StatementId);
                var statement = _context.Statements.Find(statementID);

                var cb = new BalanceViewModel
                {
                    CompteBancaire = compteBancaire,
                    NewBalance = statement.NewBalance,
                    InitialBalance = statement.InitialBalance,
                    Date = statement.Date.ToString("dd/MM/yyyy")
                };

                cb.Difference = cb.NewBalance - cb.InitialBalance;

                return cb;
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // GET: api/Balance/bis/5
        [HttpGet("bis/{id}")]
        public async Task<ActionResult<BalanceViewModel2>> GetCompteBalance2(int id)
        {
            try
            {
                var compteBancaire = await _context.CompteBancaire.FindAsync(id);
                if (compteBancaire == null) { return NotFound(); }
                var statementID = _context.Statements.Where(s => s.CompteBancaireId == compteBancaire.Id).Max(s => s.StatementId);
                var statement = _context.Statements.Find(statementID);

                var cb = compteBancaire as BalanceViewModel2;
                cb.NewBalance = statement.NewBalance;
                cb.InitialBalance = statement.InitialBalance;
                cb.Date = statement.Date.ToString("dd/MM/yyyy");
                cb.Difference = cb.NewBalance - cb.InitialBalance;

                return cb;
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
