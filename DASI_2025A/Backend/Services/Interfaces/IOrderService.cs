using Shared;

namespace Backend;

public interface IOrderService
{
  Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateDto order);
  Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync();
  Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersByBuyerIdAsync(string userId);
  Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersBySellerIdAsync(string userId);
}