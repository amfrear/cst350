using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MinesweeperMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly MinesweeperDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(MinesweeperDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Check if Username or Email already exists
                if (_context.Users.Any(u => u.Username == model.Username || u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Username or Email already exists.");
                    return View(model);
                }

                // Hash the password and store in Password field
                model.Password = HashPassword(model.Password);

                // Save user to the database
                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("RegisterSuccess");
            }

            // If we reach here, it means the model was not valid
            ModelState.AddModelError("", "An unknown error occurred. Please check the details and try again.");
            return View(model);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(password);

                // Check if the user exists
                var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

                if (user != null)
                {
                    // Store user status in session
                    Session.SetString("Username", user.Username);

                    // Set a success message
                    TempData["SuccessMessage"] = "You have successfully logged in!";

                    // Redirect to the Home page
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View();
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            Session.Clear(); // Clear session
            return RedirectToAction("Index", "Home"); // Redirect to the Home page
        }

        // Helper method to hash passwords
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
