using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rastrealo.Data;
using Rastrealo.Models;

namespace Rastrealo.Controllers
{
    public class shiftsController : Controller
    {
        private readonly RastrealoContext _context;

        public shiftsController(RastrealoContext context)
        {
            _context = context;
        }

        // GET: shifts
        public async Task<IActionResult> Index()
        {
              return _context.shift != null ? 
                          View(await _context.shift.ToListAsync()) :
                          Problem("Entity set 'RastrealoContext.shift'  is null.");
        }

        // GET: shifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.shift == null)
            {
                return NotFound();
            }

            var shift = await _context.shift
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: shifts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: shifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,shiftName,busId,driverId")] shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shift);
        }

        // GET: shifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.shift == null)
            {
                return NotFound();
            }

            var shift = await _context.shift.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            return View(shift);
        }

        // POST: shifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,shiftName,busId,driverId")] shift shift)
        {
            if (id != shift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!shiftExists(shift.Id))
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
            return View(shift);
        }

        // GET: shifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.shift == null)
            {
                return NotFound();
            }

            var shift = await _context.shift
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // POST: shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.shift == null)
            {
                return Problem("Entity set 'RastrealoContext.shift'  is null.");
            }
            var shift = await _context.shift.FindAsync(id);
            if (shift != null)
            {
                _context.shift.Remove(shift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool shiftExists(int id)
        {
          return (_context.shift?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
