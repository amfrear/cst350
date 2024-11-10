using ButtonGrid.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace ButtonGrid.Controllers
{
    public class ButtonController : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();
        string[] buttonImages = { "Blue_button.png", "Green_button.png", "Orange_button.png", "Pink_button.png" };

        public ButtonController()
        {
            if (buttons.Count == 0)
            {
                for (int i = 0; i < 25; i++)
                {
                    int number = RandomNumberGenerator.GetInt32(0, 4); // Random number between 0 and 3
                    buttons.Add(new ButtonModel(i, number, $"/img/{buttonImages[number]}"));
                }
            }
        }

        public IActionResult Index()
        {
            return View(buttons);
        }

        [HttpPost]
        public IActionResult ButtonClick(int id)
        {
            ButtonModel button = buttons.FirstOrDefault(b => b.Id == id);
            if (button != null)
            {
                button.ButtonState = (button.ButtonState + 1) % 4;
                button.ButtonImage = $"/img/{buttonImages[button.ButtonState]}";
            }

            // Check if all buttons are the same color
            if (AreAllButtonsSameColor())
            {
                TempData["SuccessMessage"] = "Congratulations! All buttons are the same color!";
            }

            return RedirectToAction("Index");
        }

        private bool AreAllButtonsSameColor()
        {
            if (buttons.Count == 0) return false;

            int firstButtonState = buttons[0].ButtonState;
            return buttons.All(button => button.ButtonState == firstButtonState);
        }
    }
}
