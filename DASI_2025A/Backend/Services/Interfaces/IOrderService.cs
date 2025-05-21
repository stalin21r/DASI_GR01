using System.Threading.Tasks;
using System.Collections.Generic;
using Shared;

namespace Backend;

public interface IOrderService
{
	Task<OrderDto> CreateOrderAsync(OrderDto orderDto, string userId);
	Task<OrderDto?> GetOrderAsync(int orderId);
	Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
	Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
	Task<decimal> GetUserBalanceAsync(string userId);
}