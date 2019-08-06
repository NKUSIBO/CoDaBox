using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inocrea.CodaBox.ApiServer.Entities;

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

    }
}
