using Shared;

namespace Backend;

public class OrderService : IOrderService
{
  private readonly IOrderRepository _orderRepository;
  public OrderService(IOrderRepository orderRepository)
  {
    _orderRepository = orderRepository;
  }

  public async Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateDto order)
  {
    var result = await _orderRepository.CreateOrderAsync(order);
    if (result == null)
    {
      throw new BadHttpRequestException("Error al crear la orden.");
    }
    ApiResponse<OrderResponseDto> response = new ApiResponse<OrderResponseDto>(
      message: "Orden creada exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync()
  {
    var result = await _orderRepository.GetAllOrdersAsync();
    if (result == null || !result.Any())
    {
      throw new KeyNotFoundException("No se encontraron Ordenes.");
    }
    ApiResponse<IEnumerable<OrderResponseDto>> response = new ApiResponse<IEnumerable<OrderResponseDto>>(
      message: "Ordenes obtenidas exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }

  public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersByBuyerIdAsync(string userId)
  {
    var result = await _orderRepository.GetOrdersByBuyerIdAsync(userId);
    if (result == null || !result.Any())
    {
      throw new KeyNotFoundException("No se encontraron Ordenes.");
    }
    ApiResponse<IEnumerable<OrderResponseDto>> response = new ApiResponse<IEnumerable<OrderResponseDto>>(
      message: "Ordenes obtenidas exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }

  public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersBySellerIdAsync(string userId)
  {
    var result = await _orderRepository.GetOrdersBySellerIdAsync(userId);
    if (result == null || !result.Any())
    {
      throw new KeyNotFoundException("No se encontraron Ordenes.");
    }
    ApiResponse<IEnumerable<OrderResponseDto>> response = new ApiResponse<IEnumerable<OrderResponseDto>>(
      message: "Ordenes obtenidas exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }
}