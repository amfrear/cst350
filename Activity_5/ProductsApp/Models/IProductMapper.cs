using ProductsApp.Models;

public interface IProductMapper
{
    ProductDTO ToDTO(ProductModel model);
    ProductModel ToModel(ProductDTO dto);
    ProductDTO ToDTO(ProductViewModel viewModel);
    ProductViewModel ToViewModel(ProductDTO dto);
}
