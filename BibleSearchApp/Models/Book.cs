namespace BibleSearchApp.Models
{
    public class Book
    {
        public int Id { get; set; } // Maps to the `id` column in `asv_books`
        public string Name { get; set; } // Maps to the `name` column in `asv_books`
        public string Testament { get; set; } // Maps to the `testament` column in `asv_books`
    }
}
