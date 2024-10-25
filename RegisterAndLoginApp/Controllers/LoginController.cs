using Microsoft.AspNetCore.Mvc;
using RegisterAndLoginApp.Models;

namespace RegisterAndLoginApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private static UserCollection _userCollection = new UserCollection();

        [HttpPost]
        [HttpPost]
        public IActionResult ProcessLogin(LoginViewModel model)
        {
            if (_userCollection.CheckCredentials(model.UserName, model.Password))
            {
                var user = _userCollection.GetAllUsers().FirstOrDefault(u => u.Username == model.UserName);
                return View("LoginSuccess", user);
            }
            else
            {
                return View("LoginFailure");
            }
        }
    }
}
