using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models; // For your User model
using MinesweeperMVC.Data;   // For your ApplicationDbContext

namespace MinesweeperMVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Hash password
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Add the new user to the database
                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Success");
            }
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
