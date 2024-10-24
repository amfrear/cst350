using Microsoft.AspNetCore.Mvc;

namespace MinesweeperMVC.Controllers
{
    public class GameController : Controller
    {
        // GET: StartGame - Restrict to logged-in users
        [HttpGet]
        public IActionResult StartGame()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                // Redirect to Login page if the user is not logged in
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
    }
}
