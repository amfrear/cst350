using AppointmentScheduler.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    public class AppointmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowAppointmentDetails(AppointmentModel appointment)
        {
            return View(appointment);
        }
    }
}
