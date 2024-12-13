using BibleSearchApp.Data;
using BibleSearchApp.Models;
using BibleSearchApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList.Extensions;
using System.Collections.Generic;
using X.PagedList;

namespace BibleSearchApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly BibleDbContext _context;

        public SearchController(BibleDbContext context)
        {
            _context = context;
        }

        // ----- Keyword Search Action -----
        public IActionResult SearchByKeyword(string keyword, int? bookId, int? chapter, string testament, int? page)
        {
            // Initialize ViewModel
            var viewModel = new SearchViewModel
            {
                Keyword = keyword,
                KeywordBookId = bookId,
                KeywordChapter = chapter,
                KeywordTestament = testament,
                KeywordPage = page
            };

            // Populate Testaments Dropdown
            viewModel.Testaments = new List<string> { "All Testaments", "Old", "New" };

            // Retrieve Books and Chapter Counts
            var books = _context.Books.ToList();
            viewModel.Books = books;

            viewModel.ChapterCounts = books.ToDictionary(
                b => b.Id,
                b => _context.Verses
                    .Where(v => v.BookId == b.Id)
                    .Select(v => v.Chapter)
                    .Distinct()
                    .Count()
            );

            // Perform the Keyword search query
            var keywordQuery = _context.Verses.Include(v => v.Book).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                keywordQuery = keywordQuery.Where(v => v.Text.Contains(keyword));
            }

            if (bookId.HasValue)
            {
                keywordQuery = keywordQuery.Where(v => v.BookId == bookId);
            }

            if (chapter.HasValue)
            {
                keywordQuery = keywordQuery.Where(v => v.Chapter == chapter);
            }

            if (!string.IsNullOrEmpty(testament) && testament != "All Testaments")
            {
                keywordQuery = keywordQuery.Where(v => v.Book.Testament == testament);
            }

            var keywordResults = keywordQuery.Select(v => new VerseResult
            {
                Id = v.Id,
                Text = v.Text,
                Chapter = v.Chapter,
                VerseNumber = v.VerseNumber,
                BookName = v.Book.Name,
                Keyword = keyword
            });

            // Pagination logic for Keyword Search
            int pageSize = 5; // Number of results per page
            int pageNumber = page ?? 1; // Current page number
            var pagedKeywordResults = keywordResults.ToPagedList(pageNumber, pageSize);

            // Initialize ReferenceResults as empty
            viewModel.ReferenceResults = new StaticPagedList<VerseResult>(new List<VerseResult>(), 1, pageSize, 0);

            // Check if the request is AJAX
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var partialViewModel = new PagedSearchResultsViewModel
                {
                    Results = pagedKeywordResults,
                    Testament = testament,
                    BookId = bookId,
                    Chapter = chapter,
                    ActionName = "SearchByKeyword",
                    Keyword = keyword // Include the keyword here
                };
                return PartialView("_SearchResultsPartial", partialViewModel);
            }

            return View(viewModel);
        }

        public IActionResult ReferenceSearch(string testament, int? bookId, int? chapter, int? page)
        {
            // Treat "All Testaments" as no filter
            if (string.IsNullOrEmpty(testament) || testament == "All Testaments")
            {
                testament = null;
            }

            // Prepare the query
            var query = _context.Verses.Include(v => v.Book).AsQueryable();

            // Apply filters if they are provided
            if (!string.IsNullOrEmpty(testament))
            {
                query = query.Where(v => v.Book.Testament == testament);
            }

            if (bookId.HasValue && bookId.Value > 0) // Ensure bookId is valid
            {
                query = query.Where(v => v.BookId == bookId.Value);
            }

            if (chapter.HasValue && chapter.Value > 0) // Ensure chapter is valid
            {
                query = query.Where(v => v.Chapter == chapter.Value);
            }

            // Select the necessary data
            var results = query.Select(v => new VerseResult
            {
                Id = v.Id,
                Text = v.Text,
                Chapter = v.Chapter,
                VerseNumber = v.VerseNumber,
                BookName = v.Book.Name
            });

            // Pagination logic
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedReferenceResults = results.ToPagedList(pageNumber, pageSize);

            // Check if the request is AJAX
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var pagedViewModel = new PagedSearchResultsViewModel
                {
                    Results = pagedReferenceResults,
                    Testament = testament ?? "All Testaments",
                    BookId = bookId ?? 0,
                    Chapter = chapter ?? 0,
                    ActionName = "ReferenceSearch"
                };

                return PartialView("_SearchResultsPartial", pagedViewModel);
            }

            // Full View
            var fullViewModel = new PagedSearchResultsViewModel
            {
                Results = pagedReferenceResults,
                Testament = testament ?? "All Testaments",
                BookId = bookId ?? 0,
                Chapter = chapter ?? 0,
                ActionName = "ReferenceSearch"
            };

            return View("ReferenceSearch", fullViewModel);
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

        [HttpPost]
        public IActionResult AddNoteAjax([FromBody] AddNoteRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                var note = new Note
                {
                    VerseId = request.VerseId,
                    Content = request.Content
                };

                _context.Notes.Add(note);
                _context.SaveChanges();
            }

            var notes = GetNotesForVerse(request.VerseId); // a method to retrieve updated notes
            return PartialView("_NotesTablePartial", notes);
        }

        [HttpPost]
        public IActionResult DeleteNoteAjax([FromBody] DeleteNoteRequest request)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == request.NoteId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
                var notes = GetNotesForVerse(note.VerseId);
                return PartialView("_NotesTablePartial", notes);
            }

            return NotFound();
        }

        private IEnumerable<NoteViewModel> GetNotesForVerse(int verseId)
        {
            return _context.Notes
                .Where(n => n.VerseId == verseId)
                .Select(n => new NoteViewModel { Id = n.Id, Content = n.Content })
                .ToList();
        }

        public class AddNoteRequest
        {
            public int VerseId { get; set; }
            public string Content { get; set; }
        }

        public class DeleteNoteRequest
        {
            public int NoteId { get; set; }
        }

        [HttpPost]
        public IActionResult EditNoteAjax([FromBody] EditNoteRequest request)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == request.NoteId);
            if (note != null)
            {
                note.Content = request.Content;
                _context.SaveChanges();

                // Retrieve the updated list of notes for the verse
                var notes = GetNotesForVerse(note.VerseId);
                // Return the partial view with updated notes
                return PartialView("_NotesTablePartial", notes);
            }

            return NotFound();
        }

        public class EditNoteRequest
        {
            public int NoteId { get; set; }
            public string Content { get; set; }
        }
    }
}
