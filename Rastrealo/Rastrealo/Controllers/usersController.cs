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
    public class usersController : Controller
    {
        private readonly RastrealoContext _context;

        public usersController(RastrealoContext context)
        {
            _context = context;
        }

        // GET: users
        public async Task<IActionResult> Index()
        {
              return _context.user != null ? 
                          View(await _context.user.ToListAsync()) :
                          Problem("Entity set 'RastrealoContext.user'  is null.");
        }

        // GET: users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public IActionResult GetStudentDetails(int studentId)
        {
            var studentDetails = _context.user
                .Where(u => u.Id == studentId)
                .Join(
                    _context.route.Join(
                        _context.driver,
                        route => route.busId,
                        driver => driver.busId,
                        (route, driver) => new { Route = route, Driver = driver }
                    ),
                    user => user.stopId,
                    combined => combined.Route.routeId,
                    (user, combined) => new
                    {
                        Student = new
                        {
                            StudentId = user.Id,
                            Shift = user.shift
                        },
                        StopId = user.stopId,
                        Route = new
                        {
                           RouteId = combined.Route.routeId,
                           RouteName = combined.Route.stopName
                        },
                        Bus = new
                        {
                            BusId = combined.Route.busId,
                            // Include other bus properties as needed
                        },
                        Driver = new
                        {
                            DriverId = combined.Driver.Id,
                            DriverName = combined.Driver.name,
                            DriverNumber = combined.Driver.phone
                            // Include other driver properties as needed
                        }
                    }
                )
                .FirstOrDefault();

            if (studentDetails == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }

            return Ok(studentDetails);
        }



        // GET: users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,password,enrollmentNo,phone,email,shift,stopId")] user user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,password,enrollmentNo,phone,email,shift,stopId")] user user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userExists(user.Id))
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
            return View(user);
        }

        // GET: users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.user == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult ValidateAdmin(String nme, String passe)
        {
            try
            {
                var admin = _context.user
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

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.user == null)
            {
                return Problem("Entity set 'RastrealoContext.user'  is null.");
            }
            var user = await _context.user.FindAsync(id);
            if (user != null)
            {
                _context.user.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool userExists(int id)
        {
          return (_context.user?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
