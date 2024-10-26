using Microsoft.AspNetCore.Mvc;
using RegisterAndLoginApp.Models;

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
                return View("LoginSuccess", user);
            }
            else
            {
                return View("LoginFailure");
            }
        }
    }
}
