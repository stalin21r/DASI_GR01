using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Shared;

namespace Frontend;

public class OrderService : IOrderService
{
  private readonly HttpClient _http;
  public OrderService(HttpClient httpClient)
  {
    _http = httpClient;
  }

  public async Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateDto order)
  {
    try
    {
      var response = await _http.PostAsJsonAsync("api/v1/Order", order);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<OrderResponseDto>(errorResponse.message)
            : new ApiResponse<OrderResponseDto>("Error al crear el producto.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderResponseDto>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<OrderResponseDto>("Error desconocido al crear el producto.");
    }
  }

  public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync()
  {
    try
    {
      var response = await _http.GetAsync("api/v1/Order");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<IEnumerable<OrderResponseDto>>(errorResponse.message)
            : new ApiResponse<IEnumerable<OrderResponseDto>>("Error al crear el producto.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderResponseDto>>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<IEnumerable<OrderResponseDto>>("Error desconocido al crear el producto.");
    }
  }

  public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersByBuyerIdAsync(string buyerId)
  {
    try
    {
      var response = await _http.GetAsync($"api/v1/Order/buyer-orders/{buyerId}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<IEnumerable<OrderResponseDto>>(errorResponse.message)
            : new ApiResponse<IEnumerable<OrderResponseDto>>("Error al crear el producto.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderResponseDto>>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<IEnumerable<OrderResponseDto>>("Error desconocido al crear el producto.");
    }
  }

  public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetOrdersBySellerIdAsync(string sellerId)
  {
    try
    {
      var response = await _http.GetAsync($"api/v1/Order/buyer-orders/{sellerId}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<IEnumerable<OrderResponseDto>>(errorResponse.message)
            : new ApiResponse<IEnumerable<OrderResponseDto>>("Error al crear el producto.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderResponseDto>>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<IEnumerable<OrderResponseDto>>("Error desconocido al crear el producto.");
    }
  }
}