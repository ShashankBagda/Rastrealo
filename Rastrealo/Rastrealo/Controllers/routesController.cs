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
    public class routesController : Controller
    {
        private readonly RastrealoContext _context;

        public routesController(RastrealoContext context)
        {
            _context = context;
        }

        // GET: routes
        public async Task<IActionResult> Index()
        {
              return _context.route != null ? 
                          View(await _context.route.ToListAsync()) :
                          Problem("Entity set 'RastrealoContext.route'  is null.");
        }

        public async Task<IActionResult> ViewList()
        {
            return _context.route != null ?
                        View(await _context.route.ToListAsync()) :
                        Problem("Entity set 'RastrealoContext.route'  is null.");
        }

        public IActionResult ViewListUser(int studentId)
        {
            var userRoutes = _context.user
           .Where(u => u.Id == studentId)
           .Join(
               _context.route,
               user => user.stopId,
               route => route.routeId,
               (user, route) => new
               {
                   UserId = user.Id,
                   UserName = user.name,
                   RouteId = route.routeId,
                   BusId = route.busId,
                   // Include other properties from User and Route models as needed
               }
           )
           .ToList();

            if (userRoutes.Count == 0)
            {
                return NotFound($"User with ID {studentId} not found or has no associated routes.");
            }

            // Get the busId corresponding to the user's routeId
            var userBusId = userRoutes.First().BusId;

            // Filter routes based on the obtained busId
            var routesForUser = _context.route
                .Where(route => route.busId == userBusId)
                .Select(route => new
                {
                    RouteId = route.routeId,
                    BusId = route.busId,
                    RouteName = route.stopName
                    // Include other route properties as needed
                })
                .ToList();

            return Ok(routesForUser);
        }


        // GET: routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.route == null)
            {
                return NotFound();
            }

            var route = await _context.route
                .FirstOrDefaultAsync(m => m.routeId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: routes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: routes/Create async Task<IActionResult>
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public string Create([Bind("routeId,busId,stopName,priority")] route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                _context.SaveChanges();
                return "yes";
            }
            return "no";
        }

        // GET: routes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.route == null)
            {
                return NotFound();
            }

            var route = await _context.route.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            return View(route);
        }

        // POST: routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("routeId,busId,stopName,priority")] route route)
        {
            if (id != route.routeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!routeExists(route.routeId))
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
            return View(route);
        }

        // GET: routes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.route == null)
            {
                return NotFound();
            }

            var route = await _context.route
                .FirstOrDefaultAsync(m => m.routeId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.route == null)
            {
                return Problem("Entity set 'RastrealoContext.route'  is null.");
            }
            var route = await _context.route.FindAsync(id);
            if (route != null)
            {
                _context.route.Remove(route);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool routeExists(int id)
        {
          return (_context.route?.Any(e => e.routeId == id)).GetValueOrDefault();
        }
    }
}
