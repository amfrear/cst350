/*!
 * BibleSearchApp
 * 
 * File: PagedSearchResultsViewModel.cs
 * Description: Defines the view model used to encapsulate paginated search results for both keyword-based and reference-based searches.
 *              Facilitates the display of search results with pagination, filtering, and contextual information.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

using X.PagedList;

namespace BibleSearchApp.ViewModels
{
    /// <summary>
    /// Represents the view model for paginated search results in the BibleSearchApp.
    /// This model encapsulates the results of a search query, along with relevant filtering and contextual information.
    /// </summary>
    public class PagedSearchResultsViewModel
    {
        /// <summary>
        /// Gets or sets the paginated list of verse results.
        /// Utilizes the <see cref="IPagedList{VerseResult}"/> interface to support pagination features.
        /// </summary>
        public IPagedList<VerseResult> Results { get; set; }

        /// <summary>
        /// Gets or sets the testament filter applied to the search results.
        /// Represents either "Old", "New", or "All Testaments".
        /// </summary>
        public string Testament { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the book filter applied to the search results.
        /// Nullable to allow for searches that do not filter by book.
        /// </summary>
        public int? BookId { get; set; }

        /// <summary>
        /// Gets or sets the chapter number filter applied to the search results.
        /// Nullable to allow for searches that do not filter by chapter.
        /// </summary>
        public int? Chapter { get; set; }

        /// <summary>
        /// Gets or sets the name of the action that initiated the search.
        /// Indicates whether the search was performed via "SearchByKeyword" or "ReferenceSearch".
        /// </summary>
        /// <remarks>
        /// This property is used to determine the context of the search and tailor the view accordingly.
        /// </remarks>
        public string ActionName { get; set; } // "SearchByKeyword" or "ReferenceSearch"

        /// <summary>
        /// Gets or sets the keyword used in a keyword-based search.
        /// Applicable only when <see cref="ActionName"/> is set to "SearchByKeyword".
        /// </summary>
        /// <remarks>
        /// This property allows the view to display the searched keyword and highlight relevant results.
        /// </remarks>
        public string Keyword { get; set; } // Only for "SearchByKeyword"
    }
}
