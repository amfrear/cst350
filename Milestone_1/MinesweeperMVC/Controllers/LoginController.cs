using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models;
using MinesweeperMVC.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MinesweeperMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Handle login submission
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists in the database
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    // Set session and redirect to success page
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("LoginSuccess");
                }
                else
                {
                    // Invalid credentials
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        // GET: Login success page
        public IActionResult LoginSuccess()
        {
            return View();
        }

        // GET: Login error page
        public IActionResult LoginError()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the Home page
            return RedirectToAction("Index", "Home");
        }
    }
}
