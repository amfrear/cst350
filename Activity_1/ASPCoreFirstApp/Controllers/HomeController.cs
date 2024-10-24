using ASPCoreFirstApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASPCoreFirstApp.Controllers
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
            // Adding the custom message with ViewData
            ViewData["Message"] = "This is a custom message from the HomeController!";
            return View();
        }

        public IActionResult About()
        {
            return View("AboutMe");
        }

        public IActionResult Projects()
        {
            return View();
        }

        public IActionResult ContactMe()
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
