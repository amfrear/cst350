using System;

namespace ProductsApp.Models
{
    // This class is tied closely to the database schema and is used for data access.
    public class ProductModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ImageURL { get; set; }

        // Constructor with parameters
        public ProductModel(int id, string name, decimal price, string description, DateTime createdAt, string imageURL)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            CreatedAt = createdAt;
            ImageURL = imageURL;
        }

        // Parameterless constructor
        public ProductModel() { }

        // Override Equals method
        public override bool Equals(object? obj)
        {
            return Equals(obj as ProductModel);
        }

        public bool Equals(ProductModel? other)
        {
            return other != null && Id == other.Id;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
