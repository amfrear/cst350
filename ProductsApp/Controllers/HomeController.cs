using Microsoft.AspNetCore.Mvc;
using ProductsApp.Models;
using ProductsApp.Services;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ProductsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly decimal _taxRate;

        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            _logger = logger;
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;

            // Get TaxRate from appsettings.json
            _taxRate = configuration.GetValue<decimal>("ProductMapperSettings:TaxRate");
        }

        // Main page
        public IActionResult Index()
        {
            return View();
        }

        // Privacy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Show Create Product Form
        public IActionResult ShowCreateProductForm()
        {
            ViewBag.TaxRate = _taxRate; // Set the tax rate from appsettings.json
            ViewBag.Images = GetImageNames(); // Fetch the list of available images
            var productViewModel = new ProductViewModel { CreatedAt = DateTime.Now };
            return View(productViewModel);
        }

        // Handle Create Product POST request
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel, IFormFile UploadedImage)
        {
            productViewModel.EstimatedTax = productViewModel.Price * _taxRate; // Use dynamic tax rate from appsettings.json

            if (UploadedImage != null && UploadedImage.Length > 0)
            {
                string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadedImage.FileName;
                string filePath = Path.Combine(imagesFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(stream);
                }

                productViewModel.ImageURL = uniqueFileName;
            }

            await _productService.AddProduct(productViewModel);
            return RedirectToAction(nameof(Index));
        }

        // Show All Products
        public async Task<IActionResult> ShowAllProducts()
        {
            IEnumerable<ProductViewModel> products = await _productService.GetAllProducts();
            return View(products);
        }

        // Helper: Get image filenames from the images folder
        private List<string> GetImageNames()
        {
            string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            return Directory.EnumerateFiles(imagesFolderPath)
                            .Select(fileName => Path.GetFileName(fileName))
                            .ToList();
        }

        public async Task<IActionResult> ShowAllProductsGrid()
        {
            IEnumerable<ProductViewModel> products = await _productService.GetAllProducts();
            return View(products);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound(); // Return 404 if product doesn't exist
            }

            await _productService.DeleteProduct(id); // Proceed with deletion
            return RedirectToAction(nameof(ShowAllProducts));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProduct(id); // Call the service method with int id
            return RedirectToAction(nameof(ShowAllProducts)); // Redirect after deletion
        }

        public async Task<IActionResult> ShowUpdateProductForm(int id)
        {
            // Fetch the product from the database using its ID
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound(); // Return a 404 error if the product doesn't exist
            }

            // Map the product data and pass it to the view
            ViewBag.TaxRate = _taxRate; // Use the tax rate from appsettings.json
            ViewBag.Images = GetImageNames(); // Fetch existing images
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductViewModel productViewModel, IFormFile UploadedImage)
        {
            productViewModel.EstimatedTax = productViewModel.Price * _taxRate; // Calculate tax dynamically

            if (UploadedImage != null && UploadedImage.Length > 0)
            {
                string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadedImage.FileName;
                string filePath = Path.Combine(imagesFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(stream);
                }

                productViewModel.ImageURL = uniqueFileName; // Update the image URL
            }

            // Save the updated product data
            await _productService.UpdateProduct(productViewModel);
            return RedirectToAction(nameof(ShowAllProducts));
        }

        public IActionResult ShowSearchForm()
        {
            var searchFor = new SearchFor();
            return View(searchFor);
        }

        [HttpPost]
        public async Task<IActionResult> SearchForProducts(SearchFor searchCriteria)
        {
            var products = await _productService.SearchForProducts(searchCriteria);
            ViewBag.SearchCriteria = searchCriteria;
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            // Fetch the product from the database using its ID
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound(); // Return a 404 error if the product doesn't exist
            }

            // Return the details view with the product data
            return View(product);
        }

        // Error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
