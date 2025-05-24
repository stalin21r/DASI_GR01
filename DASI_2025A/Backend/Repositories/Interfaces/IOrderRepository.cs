using Shared;

namespace Backend
{
    public interface IOrderRepository
    {
        Task<OrderDto> CreateAsync(OrderDto dto);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderEntity?> GetOrderAsync(int id);
        Task UpdateOrderAsync(OrderEntity order);
        Task CancelOrderAsync(OrderEntity order);
        Task SoftDeleteOrderAsync(OrderEntity order);
    }
}
