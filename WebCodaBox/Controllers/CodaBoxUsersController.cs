using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCodaBox.Models;

namespace WebCodaBox.Controllers
{
    public class CodaBoxUsersController : Controller
    {
        private readonly CodaBoxContext _context;

        public CodaBoxUsersController(CodaBoxContext context)
        {
            _context = context;
        }

        // GET: CodaBoxUsers
        public async Task<IActionResult> Index()
        {
             try {
                var codaBoxContext = _context.Users.Include(c => c.Company);
                return View(await codaBoxContext.ToListAsync());

             }
             catch (Exception ex) {

                throw ex;

             } 
        }

        // GET: CodaBoxUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codaBoxUser = await _context.Users
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codaBoxUser == null)
            {
                return NotFound();
            }

            return View(codaBoxUser);
        }

        // GET: CodaBoxUsers/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "CompanyId");
            return View();
        }

        // POST: CodaBoxUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,CompanyId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] CodaBoxUser codaBoxUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codaBoxUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "CompanyId", codaBoxUser.CompanyId);
            return View(codaBoxUser);
        }

        // GET: CodaBoxUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codaBoxUser = await _context.Users.FindAsync(id);
            if (codaBoxUser == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "CompanyId", codaBoxUser.CompanyId);
            return View(codaBoxUser);
        }

        // POST: CodaBoxUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,CompanyId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] CodaBoxUser codaBoxUser)
        {
            if (id != codaBoxUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codaBoxUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodaBoxUserExists(codaBoxUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "CompanyId", "CompanyId", codaBoxUser.CompanyId);
            return View(codaBoxUser);
        }

        // GET: CodaBoxUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codaBoxUser = await _context.Users
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codaBoxUser == null)
            {
                return NotFound();
            }

            return View(codaBoxUser);
        }

        // POST: CodaBoxUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codaBoxUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(codaBoxUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodaBoxUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
