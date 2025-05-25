using Shared;

namespace Backend
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderDto>> CreateOrderAsync(OrderDto orderDto);
    }
}
