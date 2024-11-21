using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProductsApp.Models
{
    public class ProductViewModel
    {
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; } // Price for data entry

        [Display(Name = "Price")]
        public string? FormattedPrice { get; set; } // Formatted price for display

        [Required]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } // Used for data entry

        [Display(Name = "Created At")]
        public string? FormattedDateTime { get; set; } // Formatted date for display

        public string? ImageURL { get; set; } // Image URL (nullable)

        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; } // Allows image uploads

        public decimal EstimatedTax { get; set; } // Calculated tax for the product

        [Display(Name = "Tax")]
        public string? FormattedEstimatedTax { get; set; } // Formatted tax for display
    }
}
