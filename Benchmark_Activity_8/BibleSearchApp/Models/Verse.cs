/*!
 * BibleSearchApp
 * 
 * File: Verse.cs
 * Description: Defines the Verse and Note models for the BibleSearchApp.
 *              Represents Bible verses and associated notes, including their relationships and database mappings.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibleSearchApp.Models
{
    /// <summary>
    /// Represents a verse within the Bible.
    /// </summary>
    public class Verse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the verse.
        /// Maps to the `id` column in the `asv_verses` table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the book to which this verse belongs.
        /// Maps to the `book_id` column in the `asv_verses` table.
        /// </summary>
        [Column("book_id")] // Explicitly map to the correct column name
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the chapter number of the verse.
        /// Maps to the `chapter` column in the `asv_verses` table.
        /// </summary>
        public int Chapter { get; set; }

        /// <summary>
        /// Gets or sets the verse number within the chapter.
        /// Maps to the `verse` column in the `asv_verses` table.
        /// </summary>
        [Column("verse")] // Explicitly map to the correct column name
        public int VerseNumber { get; set; }

        /// <summary>
        /// Gets or sets the text content of the verse.
        /// Maps to the `text` column in the `asv_verses` table.
        /// </summary>
        public string Text { get; set; }

        // ----- Navigation Properties -----

        /// <summary>
        /// Gets or sets the book to which this verse belongs.
        /// Represents a many-to-one relationship with the <see cref="Book"/> entity.
        /// </summary>
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        /// <summary>
        /// Gets or sets the collection of notes associated with this verse.
        /// Represents a one-to-many relationship with the <see cref="Note"/> entity.
        /// </summary>
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }

    /// <summary>
    /// Represents a note associated with a specific verse in the Bible.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Gets or sets the unique identifier for the note.
        /// Maps to the `id` column in the `notes` table.
        /// </summary>
        public int Id { get; set; } // Unique identifier for the note

        /// <summary>
        /// Gets or sets the content of the note.
        /// Maps to the `content` column in the `notes` table.
        /// </summary>
        public string Content { get; set; } // Note content

        /// <summary>
        /// Gets or sets the identifier of the verse to which this note is associated.
        /// Maps to the `verse_id` column in the `notes` table.
        /// </summary>
        public int VerseId { get; set; } // Foreign key to the verse

        /// <summary>
        /// Gets or sets the verse associated with this note.
        /// Represents a many-to-one relationship with the <see cref="Verse"/> entity.
        /// </summary>
        public Verse Verse { get; set; } // Navigation property
    }
}
