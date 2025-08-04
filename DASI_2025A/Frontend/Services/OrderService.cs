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

  public async Task<ApiResponse<PagedResult<OrderResponseDto>>> GetAllOrdersAsync(OrderQueryParams query)
  {
    try
    {
      var queryParams = $"?PageNumber={query.PageNumber}&PageSize={query.PageSize}";

      if (!string.IsNullOrWhiteSpace(query.SellerFullName))
        queryParams += $"&SellerFullName={Uri.EscapeDataString(query.SellerFullName)}";

      if (!string.IsNullOrWhiteSpace(query.BuyerFullName))
        queryParams += $"&BuyerFullName={Uri.EscapeDataString(query.BuyerFullName)}";

      if (query.StartDate.HasValue)
        queryParams += $"&StartDate={query.StartDate.Value:yyyy-MM-dd}";

      if (query.EndDate.HasValue)
        queryParams += $"&EndDate={query.EndDate.Value:yyyy-MM-dd}";

      // Asegúrate de agregar los query params a la URL
      var response = await _http.GetAsync($"api/v1/Order{queryParams}");

      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<PagedResult<OrderResponseDto>>(errorResponse.message)
          : new ApiResponse<PagedResult<OrderResponseDto>>("Error al obtener las órdenes.");
      }

      var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<OrderResponseDto>>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<PagedResult<OrderResponseDto>>("Error desconocido al obtener las órdenes.");
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