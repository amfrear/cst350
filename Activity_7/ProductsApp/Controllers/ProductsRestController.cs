using Microsoft.AspNetCore.Mvc;
using ProductsApp.Models;
using ProductsApp.Services;
using Microsoft.Extensions.Configuration;

namespace ProductsApp.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductsRestController : ControllerBase
    {
        private readonly ILogger<ProductsRestController> _logger;
        private readonly IProductService _productService;
        private readonly decimal _taxRate;

        public ProductsRestController(
            ILogger<ProductsRestController> logger,
            IProductService productService,
            IConfiguration configuration)
        {
            _logger = logger;
            _productService = productService;
            _taxRate = configuration.GetValue<decimal>("ProductMapperSettings:TaxRate");
        }

        // GET: api/productsrest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> ShowAllProducts()
        {
            // Get all products using the service
            IEnumerable<ProductViewModel> products = await _productService.GetAllProducts();

            // Return HTTP 200 with the products
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound(); // Return 404 if product not found
            }
            return Ok(product); // Return 200 with the product
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> SearchForProducts(
    [FromQuery] string searchTerm,
    [FromQuery] bool inTitle,
    [FromQuery] bool inDescription)
        {
            // Create an instance of the SearchFor class using the parameters from the URL
            var searchFor = new SearchFor
            {
                SearchTerm = searchTerm,
                InTitle = inTitle,
                InDescription = inDescription
            };

            // Perform the search using the service layer
            var searchResults = await _productService.SearchForProducts(searchFor);

            if (searchResults == null || !searchResults.Any())
            {
                return NotFound("No products found matching the search criteria.");
            }

            return Ok(searchResults); // Return HTTP 200 with the search results
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> CreateProduct([FromBody] ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Calculate and populate server-side fields
            productViewModel.EstimatedTax = productViewModel.Price * _taxRate;
            productViewModel.FormattedPrice = productViewModel.Price.ToString("C");
            productViewModel.FormattedEstimatedTax = productViewModel.EstimatedTax.ToString("C");
            productViewModel.CreatedAt = DateTime.UtcNow;
            productViewModel.FormattedDateTime = productViewModel.CreatedAt?.ToString("MMM d, yyyy");

            // Add product to the database
            await _productService.AddProduct(productViewModel);

            // Return HTTP 201 Created with the created product
            return CreatedAtAction(nameof(GetProductById), new { id = productViewModel.Id }, productViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Product with id {id} not found.");
            }

            await _productService.DeleteProduct(id);
            return NoContent(); // 204 No Content for a successful delete
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductViewModel productViewModel)
        {
            if (!int.TryParse(productViewModel.Id, out int productId) || id != productId)
            {
                return BadRequest("Product ID mismatch.");
            }

            var existingProduct = await _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with id {id} not found.");
            }

            // Update the product details
            productViewModel.EstimatedTax = productViewModel.Price * _taxRate;
            await _productService.UpdateProduct(productViewModel);

            return NoContent(); // 204 Success
        }
    }
}