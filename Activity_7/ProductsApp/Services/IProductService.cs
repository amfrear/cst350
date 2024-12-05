using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsApp.Models;

namespace ProductsApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProducts();
        Task<ProductViewModel> GetProductById(int id);
        Task<int> AddProduct(ProductViewModel product);
        Task UpdateProduct(ProductViewModel product);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductViewModel>> SearchForProducts(SearchFor searchTerm); 
    }
}
