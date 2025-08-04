using Shared;

namespace Frontend;

public interface IOrderService
{
  Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateDto order);
  Task<ApiResponse<PagedResult<OrderResponseDto>>> GetAllOrdersAsync(OrderQueryParams query);
  Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersByBuyerIdAsync(string buyerId);
  Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersBySellerIdAsync(string sellerId);
}