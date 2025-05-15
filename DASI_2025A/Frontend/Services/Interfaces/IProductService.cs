using Shared;

namespace Frontend;
public interface IProductService
{
  Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto);
  Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
  Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetAllProductsLoggerAsync();
  Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByTypeAsync(ProductType type);
  Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
  Task<ApiResponse<ProductDto>> UpdateProductAsync(UpdateProductDto productDto);
  Task<ApiResponse<ProductDto>> SellProductAsync(SellProductDto sellProductDto);
  Task<ApiResponse<bool>> DeleteProductAsync(int id);

}