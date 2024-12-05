using ProductsApp.Models;
using Microsoft.Extensions.Configuration;

namespace ProductsApp.Services
{
    public class ProductMapper : IProductMapper
    {
        // Default values for formatting and tax rate
        public string CurrencyFormat { get; set; } = "C"; // Currency format, e.g., $1,234.56
        public string DateFormat { get; set; } = "D"; // Long date format, e.g., Monday, June 15, 2029
        public decimal TaxRate { get; set; } = 0.08m; // Default tax rate of 8%

        public ProductMapper(IConfiguration configuration)
        {
            var mapperSettings = configuration.GetSection("ProductMapperSettings");
            CurrencyFormat = mapperSettings["CurrencyFormat"];
            DateFormat = mapperSettings["DateFormat"];
            TaxRate = decimal.Parse(mapperSettings["TaxRate"]);
        }

        // Constructor with parameters
        public ProductMapper(string currency, string dateFormat, decimal tax)
        {
            CurrencyFormat = currency;
            DateFormat = dateFormat;
            TaxRate = tax;
        }

        // Convert ProductModel to ProductDTO
        public ProductDTO ToDTO(ProductModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return new ProductDTO
            {
                Id = model.Id.ToString(), // Convert int to string
                Name = model.Name ?? string.Empty,
                Price = model.Price,
                Description = model.Description ?? string.Empty,
                CreatedAt = model.CreatedAt,
                ImageURL = model.ImageURL ?? string.Empty,
                EstimatedTax = model.Price * TaxRate, // Calculate tax
                FormattedPrice = model.Price.ToString(CurrencyFormat), // Format price
                FormattedDateTime = model.CreatedAt?.ToString(DateFormat) // Format date
            };
        }

        // Convert ProductDTO to ProductModel
        public ProductModel ToModel(ProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new ProductModel
            {
                Id = int.TryParse(dto.Id, out var id) ? id : 0, // Convert string to int with safety
                Name = dto.Name ?? string.Empty,
                Price = dto.Price,
                Description = dto.Description ?? string.Empty,
                CreatedAt = dto.CreatedAt,
                ImageURL = dto.ImageURL ?? string.Empty
            };
        }

        // Convert ProductViewModel to ProductDTO
        public ProductDTO ToDTO(ProductViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            return new ProductDTO
            {
                Id = viewModel.Id ?? string.Empty, // Handle null ID
                Name = viewModel.Name ?? string.Empty,
                Price = viewModel.Price,
                Description = viewModel.Description ?? string.Empty,
                CreatedAt = viewModel.CreatedAt,
                ImageURL = viewModel.ImageURL ?? string.Empty,
                EstimatedTax = viewModel.EstimatedTax, // Pre-calculated tax
                FormattedPrice = viewModel.FormattedPrice // Pre-formatted price
            };
        }

        // Convert ProductDTO to ProductViewModel
        public ProductViewModel ToViewModel(ProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new ProductViewModel
            {
                Id = dto.Id ?? string.Empty,
                Name = dto.Name ?? string.Empty,
                Price = dto.Price,
                Description = dto.Description ?? string.Empty,
                CreatedAt = dto.CreatedAt,
                ImageURL = dto.ImageURL ?? string.Empty,
                EstimatedTax = dto.EstimatedTax,
                FormattedPrice = dto.FormattedPrice,
                FormattedDateTime = dto.FormattedDateTime
            };
        }
    }
}
