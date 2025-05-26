using Shared;

namespace Backend;

public interface IProductService
{
  Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto, string userId);
  Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
  Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByTypeAsync(ProductType type);
  Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
  Task<ApiResponse<ProductDto>> UpdateProductAsync(UpdateProductDto productDto, string userId);
  Task<ApiResponse<bool>> DeleteProductAsync(int id, string userId);
}
