namespace ProductsApp.Models
{
    public class ProductDTO
    {
        public string? Id { get; set; } // String representation of the ID
        public string? Name { get; set; } // Name of the product
        public decimal Price { get; set; } // Product price
        public string? Description { get; set; } // Description of the product
        public DateTime? CreatedAt { get; set; } // Creation timestamp
        public string? ImageURL { get; set; } // URL of the product image

        // Additional properties for UI presentation
        public decimal EstimatedTax { get; set; } // Tax calculated based on the price
        public string? FormattedPrice { get; set; } // Price formatted as currency
        public string? FormattedDateTime { get; set; } // Formatted creation date
    }
}
