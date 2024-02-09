using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rastrealo.Data;
using Rastrealo.Models;

namespace Rastrealo
{
    public class adminsController : Controller
    {
        private readonly RastrealoContext _context;

        public adminsController(RastrealoContext context)
        {
            _context = context;
        }

        // GET: admins
        public async Task<IActionResult> Index()
        {
              return _context.admin != null ? 
                          View(await _context.admin.ToListAsync()) :
                          Problem("Entity set 'RastrealoContext.admin'  is null.");
        }

        // GET: admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.admin == null)
            {
                return NotFound();
            }

            var admin = await _context.admin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,password,empNo,phone,email")] admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // POST: admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult ValidateAdmin(String nme, String passe)
        {
            try
            {
                var admin = _context.admin
            .FirstOrDefault(a => a.name == nme && a.password == passe);

                if (admin == null)
                {
                    return Unauthorized("Invalid credentials. Please check your username and password.");
                }

                // If credentials are valid, return JSON data with name and id
                var responseData = new
                {
                    Name = admin.name,
                    ID = admin.Id
                };

                return Ok(responseData);
            }
            catch(Exception ex) { return NotFound($"User with ID not found or has no associated routes."); }
            
        }

        // GET: admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.admin == null)
            {
                return NotFound();
            }

            var admin = await _context.admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,password,empNo,phone,email")] admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!adminExists(admin.Id))
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
            return View(admin);
        }

        // GET: admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.admin == null)
            {
                return NotFound();
            }

            var admin = await _context.admin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.admin == null)
            {
                return Problem("Entity set 'RastrealoContext.admin'  is null.");
            }
            var admin = await _context.admin.FindAsync(id);
            if (admin != null)
            {
                _context.admin.Remove(admin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool adminExists(int id)
        {
          return (_context.admin?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
