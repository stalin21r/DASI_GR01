using Shared;

namespace Backend;

public interface IProductRepository
{
  Task<ProductDto> CreateAsync(ProductDto productDto);
  Task<IEnumerable<ProductDto>> GetAllAsync();
  Task<IEnumerable<ProductDto>> GetByTypeAsync(ProductType type);
  Task<ProductDto?> GetAsync(int id);
  Task<ProductDto?> UpdateAsync(ProductDto productDto);
  Task<bool> DeleteAsync(int id);
  Task<ProductDto?> SellProductAsync(SellProductDto sellProductDto, string userId);
}
