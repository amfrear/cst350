using System.Collections.Generic;

namespace BibleSearchApp.Models
{
    public class VerseDetailsViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Chapter { get; set; }
        public int VerseNumber { get; set; }
        public string Text { get; set; }
        public IEnumerable<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();
    }

    public class NoteViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
