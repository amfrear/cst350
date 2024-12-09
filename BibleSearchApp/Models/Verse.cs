using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibleSearchApp.Models
{
    public class Verse
    {
        public int Id { get; set; } // Maps to the `id` column in `asv_verses`

        [Column("book_id")] // Explicitly map to the correct column name
        public int BookId { get; set; }

        public int Chapter { get; set; } // Maps to the `chapter` column in `asv_verses`

        [Column("verse")] // Explicitly map to the correct column name
        public int VerseNumber { get; set; }

        public string Text { get; set; } // Maps to the `text` column in `asv_verses`

        // Navigation property for Book
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        // Collection of notes associated with the verse
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }

    public class Note
    {
        public int Id { get; set; } // Unique identifier for the note
        public string Content { get; set; } // Note content
        public int VerseId { get; set; } // Foreign key to the verse
        public Verse Verse { get; set; } // Navigation property
    }
}
