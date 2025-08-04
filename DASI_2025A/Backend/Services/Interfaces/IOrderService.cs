using Shared;

namespace Backend;

public interface IOrderService
{
  Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateDto order);
  Task<ApiResponse<PagedResult<OrderResponseDto>>> GetAllOrdersAsync(OrderQueryParams queryParams);
  Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersByBuyerIdAsync(string userId);
  Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersBySellerIdAsync(string userId);
}