using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inocrea.CodaBox.ApiServer.Entities;
using Inocrea.CodaBox.ApiServer.ViewModels;

namespace Inocrea.CodaBox.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompteBancairesController : ControllerBase
    {
        private readonly InosysDBContext _context;

        public CompteBancairesController(InosysDBContext context)
        {
            _context = context;
        }

        // GET: api/CompteBancaires
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompteBancaire>>> GetCompteBancaire()
        {
            return await _context.CompteBancaire.ToListAsync();
        }

        // GET: api/CompteBancaires/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompteBancaire>> GetCompteBancaire(int id)
        {
            var compteBancaire = await _context.CompteBancaire.FindAsync(id);
            if (compteBancaire == null) return NotFound();

            return compteBancaire;
        }

        // GET: api/CompteBancaires/5/Balance
        [HttpGet("{id}/Balance")]
        public async Task<ActionResult<BalanceViewModel>> GetCompteBalannce(int id)
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

    }
}
