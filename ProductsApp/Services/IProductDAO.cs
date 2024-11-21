using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsApp.Models;

namespace ProductsApp.Services
{
    public interface IProductDAO
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);
        Task<int> AddProduct(ProductModel product);
        Task UpdateProduct(ProductModel product);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductModel>> SearchForProductsByName(string searchTerm);
        Task<IEnumerable<ProductModel>> SearchForProductsByDescription(string searchTerm);
    }
}
