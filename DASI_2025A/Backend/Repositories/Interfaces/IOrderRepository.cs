using Shared;
namespace Backend;

public interface IOrderRepository
{
  Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto order);
  Task<IEnumerable<OrderResponseDto>> GetOrdersByBuyerIdAsync(string userId);
  Task<IEnumerable<OrderResponseDto>> GetOrdersBySellerIdAsync(string userId);
  Task<(IEnumerable<OrderResponseDto> Orders, int TotalItems)> GetAllOrdersAsync(OrderQueryParams queryParams);
}