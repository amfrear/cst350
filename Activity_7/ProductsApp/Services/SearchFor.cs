namespace ProductsApp.Models
{
    public class SearchFor
    {
        public string SearchTerm { get; set; } // Term to search for
        public bool InTitle { get; set; } // Search in product titles
        public bool InDescription { get; set; } // Search in product descriptions
    }
}
