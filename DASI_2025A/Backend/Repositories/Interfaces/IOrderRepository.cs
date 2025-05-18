using Shared;

namespace Backend;

public interface IOrderRepository
{
	Task<OrderDto> CreateAsync(OrderDto orderDto, string userId);
	Task<IEnumerable<OrderDto>> GetAllAsync();
	Task<OrderDto?> GetAsync(int id);
	Task<IEnumerable<OrderDto>> GetByUserIdAsync(string userId);
	Task<OrderDto> CreateSaleOrderAsync(int productId, uint quantity, string userId);
}
