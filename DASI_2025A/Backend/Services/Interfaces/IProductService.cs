using Shared;

namespace Backend;

public interface IProductService
{
  Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto);
  Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
  Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByTypeAsync(ProductType type);
  Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
  Task<ApiResponse<ProductDto>> UpdateProductAsync(ProductDto productDto);
  Task<ApiResponse<bool>> DeleteProductAsync(int id);
}
