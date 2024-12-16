/*!
 * BibleSearchApp
 * 
 * File: VerseDetailsViewModel.cs
 * Description: Defines the view models used to display detailed information about Bible verses and their associated notes.
 *              Includes models for displaying verse details and individual notes within the application.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

using System.Collections.Generic;

namespace BibleSearchApp.Models
{
    /// <summary>
    /// Represents the detailed information of a specific Bible verse, including its associated notes.
    /// </summary>
    public class VerseDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the verse.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the book to which this verse belongs.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the name of the book to which this verse belongs.
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// Gets or sets the chapter number of the verse.
        /// </summary>
        public int Chapter { get; set; }

        /// <summary>
        /// Gets or sets the verse number within the chapter.
        /// </summary>
        public int VerseNumber { get; set; }

        /// <summary>
        /// Gets or sets the text content of the verse.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the collection of notes associated with this verse.
        /// </summary>
        public IEnumerable<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();
    }

    /// <summary>
    /// Represents a note associated with a specific Bible verse.
    /// </summary>
    public class NoteViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the note.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the note.
        /// </summary>
        public string Content { get; set; }
    }
}
