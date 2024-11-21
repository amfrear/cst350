using ProductsApp.Models;

namespace ProductsApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDAO _productDAO; // Dependency for data access
        private readonly IProductMapper _productMapper; // Dependency for mapping logic

        // Constructor for injecting dependencies
        public ProductService(IProductDAO productDAO, IProductMapper productMapper)
        {
            _productDAO = productDAO;
            _productMapper = productMapper;
        }

        // AddProduct method implementation
        public async Task<int> AddProduct(ProductViewModel productViewModel)
        {
            // Convert from ViewModel to DTO
            var productDTO = _productMapper.ToDTO(productViewModel);

            // Convert from DTO to Model
            var productModel = _productMapper.ToModel(productDTO);

            // Add product to the database via DAO
            return await _productDAO.AddProduct(productModel);
        }

        // DeleteProduct method implementation
        public async Task DeleteProduct(int id)
        {
            await _productDAO.DeleteProduct(id);
        }

        // GetAllProducts method implementation
        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            IEnumerable<ProductModel> productModels = await _productDAO.GetAllProducts(); // Fetch products from the database
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();

            foreach (ProductModel productModel in productModels)
            {
                ProductDTO productDTO = _productMapper.ToDTO(productModel); // Map ProductModel to ProductDTO
                ProductViewModel productViewModel = _productMapper.ToViewModel(productDTO); // Map ProductDTO to ProductViewModel
                productViewModels.Add(productViewModel);
            }

            return productViewModels;
        }

        // Update GetProductById method to return ProductViewModel
        public async Task<ProductViewModel> GetProductById(int id)
        {
            var productModel = await _productDAO.GetProductById(id);

            if (productModel == null)
            {
                return null; // Handle case where the product is not found
            }

            // Map ProductModel to ProductViewModel
            var productDTO = _productMapper.ToDTO(productModel);
            return _productMapper.ToViewModel(productDTO);
        }

        public async Task<IEnumerable<ProductViewModel>> SearchForProducts(SearchFor searchCriteria)
        {
            List<ProductModel> products = new List<ProductModel>();

            if (!string.IsNullOrWhiteSpace(searchCriteria.Name))
            {
                var nameMatches = await _productDAO.SearchForProductsByName(searchCriteria.Name);
                products.AddRange(nameMatches);
            }

            if (!string.IsNullOrWhiteSpace(searchCriteria.Description))
            {
                var descriptionMatches = await _productDAO.SearchForProductsByDescription(searchCriteria.Description);
                products.AddRange(descriptionMatches);
            }

            // Remove duplicates and map to ViewModel
            return products
                .Distinct()
                .Select(product => _productMapper.ToViewModel(_productMapper.ToDTO(product)))
                .ToList();
        }

        public async Task UpdateProduct(ProductViewModel productViewModel)
        {
            // Map the ViewModel to a ProductModel using the mapper
            var productDTO = _productMapper.ToDTO(productViewModel);
            var productModel = _productMapper.ToModel(productDTO);

            // Pass the updated model to the DAO for database update
            await _productDAO.UpdateProduct(productModel);
        }
    }
}
