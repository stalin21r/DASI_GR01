using Shared;

namespace Backend;

public interface IProductRepository
{
  Task<ProductDto> CreateAsync(ProductDto productDto, string userId);
  Task<IEnumerable<ProductDto>> GetAllAsync();
  Task<IEnumerable<ProductDto>> GetByTypeAsync(ProductType type);
  Task<ProductDto?> GetAsync(int id);
  Task<ProductDto?> UpdateAsync(UpdateProductDto productDto, string userId);
  Task<bool> DeleteAsync(int id, string userId);
  Task<bool> RegisterSellProductAsync(OrderCreateDto order, string sellerId);
}
