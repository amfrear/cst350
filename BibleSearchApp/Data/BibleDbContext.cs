using BibleSearchApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BibleSearchApp.Data
{
    public class BibleDbContext : DbContext
    {
        public BibleDbContext(DbContextOptions<BibleDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Verse> Verses { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the Book model to the asv_books table
            modelBuilder.Entity<Book>().ToTable("asv_books");

            // Map the Verse model to the asv_verses table
            modelBuilder.Entity<Verse>().ToTable("asv_verses");

            // Define the relationship between Verse and Book
            modelBuilder.Entity<Verse>()
                .HasOne(v => v.Book)
                .WithMany()
                .HasForeignKey(v => v.BookId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascading delete for books

            // Map the Note model to the notes table
            modelBuilder.Entity<Note>().ToTable("notes");

            // Define the relationship between Note and Verse
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Verse)
                .WithMany(v => v.Notes)
                .HasForeignKey(n => n.VerseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete notes when a verse is deleted

            // Explicitly map VerseId to the verse_id column
            modelBuilder.Entity<Note>()
                .Property(n => n.VerseId)
                .HasColumnName("verse_id");
        }
    }
}
