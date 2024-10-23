using Microsoft.AspNetCore.Mvc;

namespace ASPCoreFirstApp.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Message()
        {
            return View();
        }

        public IActionResult Details(string name, int personality = 9)
        {
            ViewData["Name"] = name;
            ViewData["Personality"] = personality;

            return View();
        }

        public IActionResult Data(int OrderNumber, float Price, int qty)
        {
            return Json(new { OrderNumber = OrderNumber, Price = Price, Quantity = qty });
        }
    }
}
