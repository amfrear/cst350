using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RegisterAndLoginApp.Models;
using RegisterAndLoginApp.Filters; // Ensure this namespace is included
using ServiceStack.Text;
using System.Linq;

namespace RegisterAndLoginApp.Controllers
{
    public class LoginController : Controller
    {
        // Static instance of UserCollection to manage users
        static UserCollection users = new UserCollection();

        // GET: Login page
        public IActionResult Index()
        {
            return View();
        }

        // POST: Process the login form
        [HttpPost]
        public IActionResult ProcessLogin(LoginViewModel loginViewModel)
        {
            if (loginViewModel.Username == null || loginViewModel.Password == null)
            {
                ViewBag.Message = "Username and Password are required.";
                return View("Index");
            }

            var result = users.CheckCredentials(loginViewModel.Username, loginViewModel.Password);

            if (result > 0)
            {
                var user = users.GetUserById(result);

                // Set user in session as a JSON string
                var userString = ServiceStack.Text.JsonSerializer.SerializeToString(user);
                HttpContext.Session.SetString("User", userString);

                return View("LoginSuccess", user);
            }
            else
            {
                return View("LoginFailure");
            }
        }

        // GET: MembersOnly page (restricted access)
        [SessionCheckFilter] // Ensures that the user is logged in
        public IActionResult MembersOnly()
        {
            return View();
        }

        // GET: Registration page
        public IActionResult Register()
        {
            // Pass a new instance of RegisterViewModel to the view
            var registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        // POST: Process the registration form
        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new UserModel instance based on the register form data
                var newUser = new UserModel
                {
                    Username = registerViewModel.Username,
                    Salt = "defaultSalt" // Add a default salt for simplicity
                };
                newUser.SetPassword(registerViewModel.Password);

                // Assign groups to the user
                newUser.Groups = string.Join(", ", registerViewModel.Groups
                    .Where(g => g.IsSelected)
                    .Select(g => g.GroupName));

                // Add the new user to the collection
                users.AddUser(newUser);

                // Redirect to login page after successful registration
                return RedirectToAction("Index");
            }

            // If the model is invalid, re-display the form with validation messages
            return View(registerViewModel);
        }

        // GET: Logout
        public IActionResult Logout()
        {
            // Remove the "User" session key
            HttpContext.Session.Remove("User");

            // Redirect to the Login page
            return RedirectToAction("Index");
        }
    }
}
