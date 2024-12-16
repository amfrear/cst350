/*!
 * BibleSearchApp
 * 
 * File: BibleDbContext.cs
 * Description: Defines the Entity Framework Core database context for the BibleSearchApp.
 *              Manages the database sets for Books, Verses, and Notes, and configures their relationships.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

using BibleSearchApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BibleSearchApp.Data
{
    /// <summary>
    /// Represents the database context for the BibleSearchApp application.
    /// Manages the entities related to books, verses, and notes within the Bible.
    /// </summary>
    public class BibleDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BibleDbContext"/> class with specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public BibleDbContext(DbContextOptions<BibleDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Book}"/> representing the collection of books in the database.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Verse}"/> representing the collection of verses in the database.
        /// </summary>
        public DbSet<Verse> Verses { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Note}"/> representing the collection of notes in the database.
        /// </summary>
        public DbSet<Note> Notes { get; set; }

        /// <summary>
        /// Configures the entity mappings and relationships using the specified <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----- Entity Mappings -----

            // Map the Book entity to the "asv_books" table
            modelBuilder.Entity<Book>().ToTable("asv_books");

            // Map the Verse entity to the "asv_verses" table
            modelBuilder.Entity<Verse>().ToTable("asv_verses");

            // Map the Note entity to the "notes" table
            modelBuilder.Entity<Note>().ToTable("notes");

            // ----- Relationships Configuration -----

            // Define the one-to-many relationship between Book and Verse
            modelBuilder.Entity<Verse>()
                .HasOne(v => v.Book) // Each Verse has one Book
                .WithMany() // A Book can have many Verses
                .HasForeignKey(v => v.BookId) // Foreign key in Verse table
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for Books

            // Define the one-to-many relationship between Verse and Note
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Verse) // Each Note has one Verse
                .WithMany(v => v.Notes) // A Verse can have many Notes
                .HasForeignKey(n => n.VerseId) // Foreign key in Note table
                .OnDelete(DeleteBehavior.Cascade); // Cascade deletes for Notes when a Verse is deleted

            // ----- Property Configurations -----

            // Explicitly map the VerseId property in Note to the "verse_id" column
            modelBuilder.Entity<Note>()
                .Property(n => n.VerseId)
                .HasColumnName("verse_id");
        }
    }
}
