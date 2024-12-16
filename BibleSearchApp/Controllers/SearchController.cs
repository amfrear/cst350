/*!
 * BibleSearchApp
 * 
 * File: SearchController.cs
 * Description: Handles search functionalities within the BibleSearchApp, including keyword and reference searches.
 *              Manages AJAX requests for adding, deleting, and editing notes associated with Bible verses.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

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
    /// <summary>
    /// The SearchController class manages search-related actions within the BibleSearchApp.
    /// It handles both keyword-based and reference-based searches, as well as note management via AJAX.
    /// </summary>
    public class SearchController : Controller
    {
        /// <summary>
        /// The database context for accessing Bible data.
        /// </summary>
        private readonly BibleDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context for Bible data.</param>
        public SearchController(BibleDbContext context)
        {
            _context = context;
        }

        // ----- Keyword Search Action -----

        /// <summary>
        /// Performs a keyword-based search within the Bible.
        /// Allows filtering by book, chapter, and testament, and supports pagination.
        /// </summary>
        /// <param name="keyword">The keyword to search for within Bible verses.</param>
        /// <param name="bookId">Optional. The ID of the book to filter results by.</param>
        /// <param name="chapter">Optional. The chapter number to filter results by.</param>
        /// <param name="testament">Optional. The testament ("Old", "New") to filter results by.</param>
        /// <param name="page">Optional. The page number for paginated results.</param>
        /// <returns>
        /// Returns a view with the search results or a partial view if the request is AJAX.
        /// </returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the search process.</exception>
        public IActionResult SearchByKeyword(string keyword, int? bookId, int? chapter, string testament, int? page)
        {
            // Initialize ViewModel with search parameters
            var viewModel = new SearchViewModel
            {
                Keyword = keyword,
                KeywordBookId = bookId,
                KeywordChapter = chapter,
                KeywordTestament = testament,
                KeywordPage = page
            };

            // Populate Testaments Dropdown with available options
            viewModel.Testaments = new List<string> { "All Testaments", "Old", "New" };

            // Retrieve all books from the database
            var books = _context.Books.ToList();
            viewModel.Books = books;

            // Create a dictionary mapping each book ID to its chapter count
            viewModel.ChapterCounts = books.ToDictionary(
                b => b.Id,
                b => _context.Verses
                    .Where(v => v.BookId == b.Id)
                    .Select(v => v.Chapter)
                    .Distinct()
                    .Count()
            );

            // Begin constructing the keyword search query
            var keywordQuery = _context.Verses.Include(v => v.Book).AsQueryable();

            // Apply keyword filter if provided
            if (!string.IsNullOrEmpty(keyword))
            {
                keywordQuery = keywordQuery.Where(v => v.Text.Contains(keyword));
            }

            // Apply book filter if provided
            if (bookId.HasValue)
            {
                keywordQuery = keywordQuery.Where(v => v.BookId == bookId);
            }

            // Apply chapter filter if provided
            if (chapter.HasValue)
            {
                keywordQuery = keywordQuery.Where(v => v.Chapter == chapter);
            }

            // Apply testament filter if provided and not set to "All Testaments"
            if (!string.IsNullOrEmpty(testament) && testament != "All Testaments")
            {
                keywordQuery = keywordQuery.Where(v => v.Book.Testament == testament);
            }

            // Project the query results into VerseResult view models
            var keywordResults = keywordQuery.Select(v => new VerseResult
            {
                Id = v.Id,
                Text = v.Text,
                Chapter = v.Chapter,
                VerseNumber = v.VerseNumber,
                BookName = v.Book.Name,
                Keyword = keyword
            });

            // Define pagination parameters
            int pageSize = 5; // Number of results per page
            int pageNumber = page ?? 1; // Current page number
            var pagedKeywordResults = keywordResults.ToPagedList(pageNumber, pageSize);

            // Initialize ReferenceResults as empty for this action
            viewModel.ReferenceResults = new StaticPagedList<VerseResult>(new List<VerseResult>(), 1, pageSize, 0);

            // Check if the request is an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Create a partial view model with paginated results
                var partialViewModel = new PagedSearchResultsViewModel
                {
                    Results = pagedKeywordResults,
                    Testament = testament,
                    BookId = bookId,
                    Chapter = chapter,
                    ActionName = "SearchByKeyword",
                    Keyword = keyword // Include the keyword in the view model
                };
                return PartialView("_SearchResultsPartial", partialViewModel); // Return partial view for AJAX
            }

            return View(viewModel); // Return full view for standard requests
        }

        /// <summary>
        /// Performs a reference-based search within the Bible.
        /// Allows filtering by testament, book, and chapter, and supports pagination.
        /// </summary>
        /// <param name="testament">Optional. The testament ("Old", "New") to filter results by.</param>
        /// <param name="bookId">Optional. The ID of the book to filter results by.</param>
        /// <param name="chapter">Optional. The chapter number to filter results by.</param>
        /// <param name="page">Optional. The page number for paginated results.</param>
        /// <returns>
        /// Returns a view with the search results or a partial view if the request is AJAX.
        /// </returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the search process.</exception>
        public IActionResult ReferenceSearch(string testament, int? bookId, int? chapter, int? page)
        {
            // Treat "All Testaments" as no filter by setting testament to null
            if (string.IsNullOrEmpty(testament) || testament == "All Testaments")
            {
                testament = null;
            }

            // Begin constructing the reference search query
            var query = _context.Verses.Include(v => v.Book).AsQueryable();

            // Apply testament filter if provided
            if (!string.IsNullOrEmpty(testament))
            {
                query = query.Where(v => v.Book.Testament == testament);
            }

            // Apply book filter if provided and valid
            if (bookId.HasValue && bookId.Value > 0)
            {
                query = query.Where(v => v.BookId == bookId.Value);
            }

            // Apply chapter filter if provided and valid
            if (chapter.HasValue && chapter.Value > 0)
            {
                query = query.Where(v => v.Chapter == chapter.Value);
            }

            // Project the query results into VerseResult view models
            var results = query.Select(v => new VerseResult
            {
                Id = v.Id,
                Text = v.Text,
                Chapter = v.Chapter,
                VerseNumber = v.VerseNumber,
                BookName = v.Book.Name
            });

            // Define pagination parameters
            int pageSize = 5; // Number of results per page
            int pageNumber = page ?? 1; // Current page number
            var pagedReferenceResults = results.ToPagedList(pageNumber, pageSize);

            // Check if the request is an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Create a partial view model with paginated results
                var pagedViewModel = new PagedSearchResultsViewModel
                {
                    Results = pagedReferenceResults,
                    Testament = testament ?? "All Testaments",
                    BookId = bookId ?? 0,
                    Chapter = chapter ?? 0,
                    ActionName = "ReferenceSearch"
                };

                return PartialView("_SearchResultsPartial", pagedViewModel); // Return partial view for AJAX
            }

            // Create a full view model with paginated results
            var fullViewModel = new PagedSearchResultsViewModel
            {
                Results = pagedReferenceResults,
                Testament = testament ?? "All Testaments",
                BookId = bookId ?? 0,
                Chapter = chapter ?? 0,
                ActionName = "ReferenceSearch"
            };

            return View("ReferenceSearch", fullViewModel); // Return full view for standard requests
        }

        /// <summary>
        /// Retrieves the details of a specific verse, including associated notes.
        /// </summary>
        /// <param name="id">The ID of the verse to retrieve details for.</param>
        /// <returns>
        /// Returns a view with the verse details or a NotFound result if the verse does not exist.
        /// </returns>
        /// <exception cref="NotFoundResult">Thrown when the specified verse ID does not correspond to any verse.</exception>
        public IActionResult VerseDetails(int id)
        {
            // Retrieve the verse entity along with related book and notes
            var verseEntity = _context.Verses
                .Include(v => v.Book)
                .Include(v => v.Notes)
                .FirstOrDefault(v => v.Id == id);

            // Return 404 Not Found if the verse does not exist
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

            return View(viewModel); // Return the view with verse details
        }

        /// <summary>
        /// Adds a new note to a specific verse via an AJAX request.
        /// </summary>
        /// <param name="request">The request containing the verse ID and note content.</param>
        /// <returns>
        /// Returns a partial view with the updated list of notes or a NotFound result if the verse does not exist.
        /// </returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the note addition process.</exception>
        [HttpPost]
        public IActionResult AddNoteAjax([FromBody] AddNoteRequest request)
        {
            // Validate that the note content is not empty or whitespace
            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                // Create a new Note entity with the provided data
                var note = new Note
                {
                    VerseId = request.VerseId,
                    Content = request.Content
                };

                _context.Notes.Add(note); // Add the note to the context
                _context.SaveChanges(); // Persist changes to the database
            }

            // Retrieve the updated list of notes for the specified verse
            var notes = GetNotesForVerse(request.VerseId);
            return PartialView("_NotesTablePartial", notes); // Return partial view with updated notes
        }

        /// <summary>
        /// Deletes an existing note via an AJAX request.
        /// </summary>
        /// <param name="request">The request containing the ID of the note to delete.</param>
        /// <returns>
        /// Returns a partial view with the updated list of notes or a NotFound result if the note does not exist.
        /// </returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the note deletion process.</exception>
        [HttpPost]
        public IActionResult DeleteNoteAjax([FromBody] DeleteNoteRequest request)
        {
            // Find the note entity based on the provided note ID
            var note = _context.Notes.FirstOrDefault(n => n.Id == request.NoteId);
            if (note != null)
            {
                _context.Notes.Remove(note); // Remove the note from the context
                _context.SaveChanges(); // Persist changes to the database

                // Retrieve the updated list of notes for the associated verse
                var notes = GetNotesForVerse(note.VerseId);
                return PartialView("_NotesTablePartial", notes); // Return partial view with updated notes
            }

            return NotFound(); // Return 404 Not Found if the note does not exist
        }

        /// <summary>
        /// Edits an existing note via an AJAX request.
        /// </summary>
        /// <param name="request">The request containing the note ID and the new content.</param>
        /// <returns>
        /// Returns a partial view with the updated list of notes or a NotFound result if the note does not exist.
        /// </returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the note editing process.</exception>
        [HttpPost]
        public IActionResult EditNoteAjax([FromBody] EditNoteRequest request)
        {
            // Find the note entity based on the provided note ID
            var note = _context.Notes.FirstOrDefault(n => n.Id == request.NoteId);
            if (note != null)
            {
                note.Content = request.Content; // Update the note content
                _context.SaveChanges(); // Persist changes to the database

                // Retrieve the updated list of notes for the associated verse
                var notes = GetNotesForVerse(note.VerseId);
                // Return the partial view with updated notes
                return PartialView("_NotesTablePartial", notes);
            }

            return NotFound(); // Return 404 Not Found if the note does not exist
        }

        /// <summary>
        /// Retrieves the list of notes associated with a specific verse.
        /// </summary>
        /// <param name="verseId">The ID of the verse to retrieve notes for.</param>
        /// <returns>A collection of <see cref="NoteViewModel"/> representing the notes.</returns>
        private IEnumerable<NoteViewModel> GetNotesForVerse(int verseId)
        {
            // Query the database for notes related to the specified verse
            return _context.Notes
                .Where(n => n.VerseId == verseId)
                .Select(n => new NoteViewModel { Id = n.Id, Content = n.Content })
                .ToList();
        }

        /// <summary>
        /// Represents a request to add a new note.
        /// </summary>
        public class AddNoteRequest
        {
            /// <summary>
            /// Gets or sets the ID of the verse to which the note is associated.
            /// </summary>
            public int VerseId { get; set; }

            /// <summary>
            /// Gets or sets the content of the note.
            /// </summary>
            public string Content { get; set; }
        }

        /// <summary>
        /// Represents a request to delete an existing note.
        /// </summary>
        public class DeleteNoteRequest
        {
            /// <summary>
            /// Gets or sets the ID of the note to delete.
            /// </summary>
            public int NoteId { get; set; }
        }

        /// <summary>
        /// Represents a request to edit an existing note.
        /// </summary>
        public class EditNoteRequest
        {
            /// <summary>
            /// Gets or sets the ID of the note to edit.
            /// </summary>
            public int NoteId { get; set; }

            /// <summary>
            /// Gets or sets the new content for the note.
            /// </summary>
            public string Content { get; set; }
        }
    }
}
