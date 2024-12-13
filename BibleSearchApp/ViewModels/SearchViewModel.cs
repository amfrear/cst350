using X.PagedList;
using System.Collections.Generic;
using BibleSearchApp.Models;

namespace BibleSearchApp.ViewModels
{
    public class SearchViewModel
    {
        // ----- Keyword Search Parameters -----
        public string Keyword { get; set; }
        public int? KeywordBookId { get; set; }
        public int? KeywordChapter { get; set; }
        public string KeywordTestament { get; set; }
        public int? KeywordPage { get; set; }

        // ----- Reference Search Parameters -----
        public string ReferenceTestament { get; set; }
        public int? ReferenceBookId { get; set; }
        public int? ReferenceChapter { get; set; }
        public int? ReferencePage { get; set; }

        // ----- Dropdown Data -----
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<string> Testaments { get; set; }
        public Dictionary<int, int> ChapterCounts { get; set; }

        // ----- Search Results -----
        public IPagedList<VerseResult> KeywordResults { get; set; }
        public IPagedList<VerseResult> ReferenceResults { get; set; }

        // ----- Indicators -----
        public bool IsKeywordSearchPerformed => !string.IsNullOrEmpty(Keyword) || KeywordBookId.HasValue || KeywordChapter.HasValue || !string.IsNullOrEmpty(KeywordTestament);
        public bool IsReferenceSearchPerformed => !string.IsNullOrEmpty(ReferenceTestament) || ReferenceBookId.HasValue || ReferenceChapter.HasValue;
    }

    public class VerseResult
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Chapter { get; set; }
        public int VerseNumber { get; set; }
        public string BookName { get; set; }
        public string Keyword { get; set; }
    }
}
