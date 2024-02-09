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
    public class busesController : Controller
    {
        private readonly RastrealoContext _context;

        public busesController(RastrealoContext context)
        {
            _context = context;
        }

        // GET: buses
        public async Task<IActionResult> Index()
        {
              return _context.bus != null ? 
                          View(await _context.bus.ToListAsync()) :
                          Problem("Entity set 'RastrealoContext.bus'  is null.");
        }

        // GET: buses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.bus == null)
            {
                return NotFound();
            }

            var bus = await _context.bus
                .FirstOrDefaultAsync(m => m.busId == id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        public async Task<IActionResult> GetBusLocation(int busId)
        {
            try
            {
                // Find the bus by busId
                var bus = await _context.bus.FindAsync(busId);

                if (bus == null)
                {
                    return NotFound($"Bus with ID {busId} not found.");
                }

                // Return JSON data with Latitude and Longitude attributes
                var busLocation = new
                {
                    Latitude = bus.latitude,
                    Longitude = bus.longitude
                };

                return Ok(busLocation);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> GetBusRoute(int busId)
        {
            try
            {
                // Retrieve routes where BusId matches the provided busId
                var routes = await _context.route
                    .Where(r => r.busId == busId)
                    .ToListAsync();

                if (routes == null || !routes.Any())
                {
                    return NotFound($"No routes found for Bus with ID {busId}.");
                }

                return Ok(routes);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: buses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: buses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("busId,numberPlate,shift,latitude,longitude,passengers,status")] bus bus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bus);
        }

        // GET: buses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.bus == null)
            {
                return NotFound();
            }

            var bus = await _context.bus.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        // POST: buses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("busId,numberPlate,shift,latitude,longitude,passengers,status")] bus bus)
        {
            if (id != bus.busId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!busExists(bus.busId))
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
            return View(bus);
        }

        // GET: buses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.bus == null)
            {
                return NotFound();
            }

            var bus = await _context.bus
                .FirstOrDefaultAsync(m => m.busId == id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        // POST: buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.bus == null)
            {
                return Problem("Entity set 'RastrealoContext.bus'  is null.");
            }
            var bus = await _context.bus.FindAsync(id);
            if (bus != null)
            {
                _context.bus.Remove(bus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> updateLocation(int busId, string latitude, string longitude)
        {
            try
            {
                var bus = await _context.bus.FindAsync(busId);

                if (bus == null)
                {
                    return NotFound($"Bus with ID {busId} not found.");
                }

                // Update the latitude and longitude
                bus.latitude = latitude;
                bus.longitude = longitude;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok($"Bus location updated successfully. {latitude} {longitude}");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }
        }

        private bool busExists(int id)
        {
          return (_context.bus?.Any(e => e.busId == id)).GetValueOrDefault();
        }
    }
}
