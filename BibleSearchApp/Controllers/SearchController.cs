using BibleSearchApp.Data;
using BibleSearchApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BibleSearchApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly BibleDbContext _context;

        public SearchController(BibleDbContext context)
        {
            _context = context;
        }

        public IActionResult SearchByKeyword(string keyword, int? bookId, int? chapter, string testament)
        {
            // Populate books for dropdown filters
            var books = _context.Books.ToList();
            ViewBag.Books = books;

            // Calculate chapter counts for each book
            var chapterCounts = books.ToDictionary(
                b => b.Id,
                b => _context.Verses
                    .Where(v => v.BookId == b.Id)
                    .Select(v => v.Chapter)
                    .Distinct()
                    .Count()
            );
            ViewBag.ChapterCounts = chapterCounts;

            // Populate testament dropdown
            ViewBag.Testaments = new List<string> { "All Testaments", "Old", "New" };

            // Check if a search was performed
            ViewData["SearchPerformed"] = !string.IsNullOrEmpty(keyword) || bookId.HasValue || chapter.HasValue || !string.IsNullOrEmpty(testament);
            ViewData["SearchedKeyword"] = keyword;
            ViewData["SearchedBook"] = bookId.HasValue ? books.FirstOrDefault(b => b.Id == bookId)?.Name : null;
            ViewData["SearchedChapter"] = chapter?.ToString();
            ViewData["SearchedTestament"] = testament;

            // Perform the search query
            var query = _context.Verses.Include(v => v.Book).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(v => v.Text.Contains(keyword));
            }

            if (bookId.HasValue)
            {
                query = query.Where(v => v.BookId == bookId);
            }

            if (chapter.HasValue)
            {
                query = query.Where(v => v.Chapter == chapter);
            }

            if (!string.IsNullOrEmpty(testament) && testament != "All Testaments")
            {
                query = query.Where(v => v.Book.Testament == testament);
            }

            var results = query.Select(v => new
            {
                v.Id,
                v.Text,
                v.Chapter,
                v.VerseNumber,
                BookName = v.Book.Name,
                Keyword = keyword
            }).ToList();

            return View(results);
        }

        public IActionResult ReferenceSearch(int? bookId, int? chapter)
        {
            if (!bookId.HasValue || !chapter.HasValue)
            {
                return RedirectToAction("SearchByKeyword"); // Redirect to search page if parameters are missing
            }

            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return RedirectToAction("SearchByKeyword"); // Redirect if the book is invalid
            }

            var verses = _context.Verses
                .Where(v => v.BookId == bookId && v.Chapter == chapter)
                .OrderBy(v => v.VerseNumber)
                .Select(v => new
                {
                    v.Id,
                    v.Text,
                    v.VerseNumber,
                    BookName = book.Name,
                    Chapter = v.Chapter
                })
                .ToList();

            ViewBag.BookName = book.Name;
            ViewBag.Chapter = chapter;

            return View(verses);
        }

        public IActionResult VerseDetails(int id)
        {
            var verseEntity = _context.Verses
                .Include(v => v.Book)
                .Include(v => v.Notes)
                .FirstOrDefault(v => v.Id == id);

            if (verseEntity == null)
            {
                return NotFound();
            }

            // Map the Verse entity to the VerseDetailsViewModel
            var viewModel = new VerseDetailsViewModel
            {
                Id = verseEntity.Id,
                BookId = verseEntity.BookId,
                BookName = verseEntity.Book.Name,
                Chapter = verseEntity.Chapter,
                VerseNumber = verseEntity.VerseNumber,
                Text = verseEntity.Text,
                Notes = verseEntity.Notes.Select(n => new NoteViewModel
                {
                    Id = n.Id,
                    Content = n.Content
                }).ToList()
            };

            return View(viewModel);
        }

        // Add a method to handle adding a new note
        [HttpPost]
        public IActionResult AddNote(int verseId, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var note = new Note
                {
                    VerseId = verseId,
                    Content = content
                };

                _context.Notes.Add(note);
                _context.SaveChanges();
            }

            return RedirectToAction("VerseDetails", new { id = verseId });
        }      

        public IActionResult EditNote(int noteId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        [HttpPost]
        public IActionResult EditNote(Note updatedNote)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == updatedNote.Id);
            if (note != null)
            {
                note.Content = updatedNote.Content;
                _context.SaveChanges();
                return RedirectToAction("VerseDetails", new { id = note.VerseId });
            }

            return NotFound(); // Return a 404 if the note is not found
        }

        [HttpPost]
        public IActionResult DeleteNote(int noteId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
            }
            return RedirectToAction("VerseDetails", new { id = note.VerseId });
        }
    }
}
