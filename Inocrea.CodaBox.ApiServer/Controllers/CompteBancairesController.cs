using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

            if (compteBancaire == null)
            {
                return NotFound();
            }

            return compteBancaire;
        }

        // PUT: api/CompteBancaires/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompteBancaire(int id, CompteBancaire compteBancaire)
        {
            if (id != compteBancaire.Id)
            {
                return BadRequest();
            }

            _context.Entry(compteBancaire).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompteBancaireExists(id))
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

        // POST: api/CompteBancaires
        [HttpPost]
        public async Task<ActionResult<CompteBancaire>> PostCompteBancaire(CompteBancaire compteBancaire)
        {
            _context.CompteBancaire.Add(compteBancaire);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompteBancaire", new { id = compteBancaire.Id }, compteBancaire);
        }

        // DELETE: api/CompteBancaires/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CompteBancaire>> DeleteCompteBancaire(int id)
        {
            var compteBancaire = await _context.CompteBancaire.FindAsync(id);
            if (compteBancaire == null)
            {
                return NotFound();
            }

            _context.CompteBancaire.Remove(compteBancaire);
            await _context.SaveChangesAsync();

            return compteBancaire;
        }

        private bool CompteBancaireExists(int id)
        {
            return _context.CompteBancaire.Any(e => e.Id == id);
        }
    }
}
