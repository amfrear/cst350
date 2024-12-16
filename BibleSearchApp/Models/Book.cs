/*!
 * BibleSearchApp
 * 
 * File: Book.cs
 * Description: Represents a book within the Bible, including its name and testament classification.
 *              Maps to the `asv_books` table in the database.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

namespace BibleSearchApp.Models
{
    /// <summary>
    /// Represents a book within the Bible.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier for the book.
        /// Maps to the `id` column in the `asv_books` table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the book.
        /// Maps to the `name` column in the `asv_books` table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the testament classification of the book.
        /// Maps to the `testament` column in the `asv_books` table.
        /// </summary>
        public string Testament { get; set; }
    }
}
