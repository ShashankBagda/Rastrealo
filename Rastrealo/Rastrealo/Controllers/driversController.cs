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
    public class driversController : Controller
    {
        private readonly RastrealoContext _context;

        public driversController(RastrealoContext context)
        {
            _context = context;
        }

        // GET: drivers
        public async Task<IActionResult> Index()
        {
              return _context.driver != null ? 
                          View(await _context.driver.ToListAsync()) :
                          Problem("Entity set 'RastrealoContext.driver'  is null.");
        }

        // GET: drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.driver == null)
            {
                return NotFound();
            }

            var driver = await _context.driver
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: drivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,password,empNo,phone,email,busId,shiftNo")] driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.driver == null)
            {
                return NotFound();
            }

            var driver = await _context.driver.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        // POST: drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,password,empNo,phone,email,busId,shiftNo")] driver driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!driverExists(driver.Id))
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
            return View(driver);
        }

        [HttpPost]
        public IActionResult ValidateAdmin(String nme, String passe)
        {
            try
            {
                var admin = _context.driver
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
            catch (Exception ex) { return NotFound($"User with ID not found or has no associated routes."); }
        }

        // GET: drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.driver == null)
            {
                return NotFound();
            }

            var driver = await _context.driver
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        public IActionResult GetDriverData(int driverId)
        {
            var routeList = _context.shift.Where(s => s.driverId == driverId).ToList();

            return Ok(routeList);
        }

        public IActionResult GetDriverBus(int driverId)
        {
            try
            {
                // Find the driver by driverId
                var driver = _context.driver.Find(driverId);

                if (driver == null)
                {
                    return NotFound($"Driver with ID {driverId} not found.");
                }

                // Get the busId linked with the driver
                var busId = driver.busId;

                if (busId != null )
                {
                    return Ok(new { busId = busId });
                }
                else
                {
                    return NotFound("No bus linked with the specified driver.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST: drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.driver == null)
            {
                return Problem("Entity set 'RastrealoContext.driver'  is null.");
            }
            var driver = await _context.driver.FindAsync(id);
            if (driver != null)
            {
                _context.driver.Remove(driver);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool driverExists(int id)
        {
          return (_context.driver?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
