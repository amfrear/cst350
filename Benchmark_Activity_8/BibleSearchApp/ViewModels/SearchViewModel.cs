/*!
 * BibleSearchApp
 * 
 * File: SearchViewModel.cs
 * Description: Defines the view models used for managing search parameters and displaying search results within the BibleSearchApp.
 *              Includes models for both keyword-based and reference-based searches, along with their associated results and dropdown data.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

using X.PagedList;
using System.Collections.Generic;
using BibleSearchApp.Models;

namespace BibleSearchApp.ViewModels
{
    /// <summary>
    /// Represents the view model for managing search parameters and displaying search results in the BibleSearchApp.
    /// Handles both keyword-based and reference-based search functionalities.
    /// </summary>
    public class SearchViewModel
    {
        // ----- Keyword Search Parameters -----

        /// <summary>
        /// Gets or sets the keyword used for keyword-based searches.
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the book used to filter keyword-based search results.
        /// Nullable to allow searches without a specific book filter.
        /// </summary>
        public int? KeywordBookId { get; set; }

        /// <summary>
        /// Gets or sets the chapter number used to filter keyword-based search results.
        /// Nullable to allow searches without a specific chapter filter.
        /// </summary>
        public int? KeywordChapter { get; set; }

        /// <summary>
        /// Gets or sets the testament ("Old", "New") used to filter keyword-based search results.
        /// </summary>
        public string KeywordTestament { get; set; }

        /// <summary>
        /// Gets or sets the page number for paginated keyword-based search results.
        /// Nullable to allow default pagination.
        /// </summary>
        public int? KeywordPage { get; set; }

        // ----- Reference Search Parameters -----

        /// <summary>
        /// Gets or sets the testament ("Old", "New") used to filter reference-based search results.
        /// </summary>
        public string ReferenceTestament { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the book used to filter reference-based search results.
        /// Nullable to allow searches without a specific book filter.
        /// </summary>
        public int? ReferenceBookId { get; set; }

        /// <summary>
        /// Gets or sets the chapter number used to filter reference-based search results.
        /// Nullable to allow searches without a specific chapter filter.
        /// </summary>
        public int? ReferenceChapter { get; set; }

        /// <summary>
        /// Gets or sets the page number for paginated reference-based search results.
        /// Nullable to allow default pagination.
        /// </summary>
        public int? ReferencePage { get; set; }

        // ----- Dropdown Data -----

        /// <summary>
        /// Gets or sets the collection of books available for selection in search filters.
        /// </summary>
        public IEnumerable<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets the collection of testaments ("Old", "New", "All Testaments") available for selection in search filters.
        /// </summary>
        public IEnumerable<string> Testaments { get; set; }

        /// <summary>
        /// Gets or sets the dictionary mapping each book's ID to its total number of chapters.
        /// Used to dynamically populate chapter dropdowns based on selected books.
        /// </summary>
        public Dictionary<int, int> ChapterCounts { get; set; }

        // ----- Search Results -----

        /// <summary>
        /// Gets or sets the paginated list of verse results for keyword-based searches.
        /// Utilizes the <see cref="IPagedList{VerseResult}"/> interface to support pagination features.
        /// </summary>
        public IPagedList<VerseResult> KeywordResults { get; set; }

        /// <summary>
        /// Gets or sets the paginated list of verse results for reference-based searches.
        /// Utilizes the <see cref="IPagedList{VerseResult}"/> interface to support pagination features.
        /// </summary>
        public IPagedList<VerseResult> ReferenceResults { get; set; }

        // ----- Indicators -----

        /// <summary>
        /// Determines whether a keyword-based search has been performed based on the presence of search parameters.
        /// </summary>
        public bool IsKeywordSearchPerformed =>
            !string.IsNullOrEmpty(Keyword) ||
            KeywordBookId.HasValue ||
            KeywordChapter.HasValue ||
            !string.IsNullOrEmpty(KeywordTestament);

        /// <summary>
        /// Determines whether a reference-based search has been performed based on the presence of search parameters.
        /// </summary>
        public bool IsReferenceSearchPerformed =>
            !string.IsNullOrEmpty(ReferenceTestament) ||
            ReferenceBookId.HasValue ||
            ReferenceChapter.HasValue;
    }

    /// <summary>
    /// Represents the result of a search query, encapsulating details about a specific Bible verse.
    /// </summary>
    public class VerseResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the verse.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text content of the verse.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the chapter number of the verse.
        /// </summary>
        public int Chapter { get; set; }

        /// <summary>
        /// Gets or sets the verse number within the chapter.
        /// </summary>
        public int VerseNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the book to which this verse belongs.
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// Gets or sets the keyword associated with this verse, if applicable.
        /// </summary>
        public string Keyword { get; set; }
    }
}
