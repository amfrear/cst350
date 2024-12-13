using X.PagedList;

namespace BibleSearchApp.ViewModels
{
    public class PagedSearchResultsViewModel
    {
        public IPagedList<VerseResult> Results { get; set; }
        public string Testament { get; set; }
        public int? BookId { get; set; }
        public int? Chapter { get; set; }
        public string ActionName { get; set; } // "SearchByKeyword" or "ReferenceSearch"
        public string Keyword { get; set; } // Only for "SearchByKeyword"
    }
}
