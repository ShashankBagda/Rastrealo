using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rastrealo.Models;

namespace Rastrealo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult student()
        {
            return View();
        }

        public IActionResult admin()
        {
            return View();
        }

        public IActionResult driver()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
