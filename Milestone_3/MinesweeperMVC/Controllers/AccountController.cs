using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MinesweeperMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly MinesweeperDbContext _context;

        public AccountController(MinesweeperDbContext context)
        {
            _context = context;
        }

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
            if (model == null)
            {
                ModelState.AddModelError("", "Invalid user data.");
                return View();
            }

            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username || u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Username or Email already exists.");
                    return View(model);
                }

                model.Password = HashPassword(model.Password); // Hash the password
                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("RegisterSuccess");
            }

            ModelState.AddModelError("", "An unknown error occurred. Please try again.");
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
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            var hashedPassword = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user != null)
            {
                // Create user claims
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                // Add a success message
                TempData["SuccessMessage"] = $"Welcome, {user.Username}!";

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            // Use the custom scheme for SignOutAsync
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
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
