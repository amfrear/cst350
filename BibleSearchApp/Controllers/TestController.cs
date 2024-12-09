using BibleSearchApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace BibleSearchApp.Controllers
{
    public class TestController : Controller
    {
        private readonly BibleDbContext _context;

        public TestController(BibleDbContext context)
        {
            _context = context;
        }

        // Test fetching all books
        public IActionResult Books()
        {
            var books = _context.Books.ToList();
            return Json(books); // Return the books as JSON
        }

        // Test fetching all verses
        public IActionResult Verses()
        {
            var verses = _context.Verses.Take(10).ToList(); // Limit to 10 verses
            return Json(verses); // Return the verses as JSON
        }
    }
}
