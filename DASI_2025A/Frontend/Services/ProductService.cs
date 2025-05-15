using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Shared;

namespace Frontend;
public class ProductService : IProductService
{
  private readonly HttpClient _http;
  public ProductService(HttpClient httpClient)
  {
    _http = httpClient;
  }
  public async Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto)
  {
    try
    {
      var response = await _http.PostAsJsonAsync("api/v1/Product", productDto);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<ProductDto>(errorResponse.message)
            : new ApiResponse<ProductDto>("Error al crear el producto.");
      }

      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
      };
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>(options);
      return result!;
    }
    catch
    {
      return new ApiResponse<ProductDto>("Error desconocido al crear el producto.");
    }
  }

  public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
  {
    try
    {
      var response = await _http.GetAsync("api/v1/Product");

      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<IEnumerable<ProductDto>>(errorResponse.message)
            : new ApiResponse<IEnumerable<ProductDto>>("Error al obtener los productos.");
      }

      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
      };

      var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductDto>>>(options);
      Console.WriteLine(result?.data);

      return new ApiResponse<IEnumerable<ProductDto>>(
          result!.message,
          result.data,
          result.totalRecords
      );
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<IEnumerable<ProductDto>>("Error desconocido al obtener los productos.");

    }
  }

  public async Task<ApiResponse<IEnumerable<ProductLoggerDto>>> GetAllProductsLoggerAsync()
  {
    try
    {
      var response = await _http.GetAsync("api/v1/ProductLogger");

      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<IEnumerable<ProductLoggerDto>>(errorResponse.message)
            : new ApiResponse<IEnumerable<ProductLoggerDto>>("Error al obtener el historial.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ProductLoggerDto>>>();

      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<IEnumerable<ProductLoggerDto>>("Error desconocido al obtener el historial.");
    }
  }


  public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
  {
    try
    {
      var response = await _http.GetAsync($"api/v1/Product/{id}");

      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<ProductDto>(errorResponse.message)
            : new ApiResponse<ProductDto>("Error al obtener el producto.");
      }
      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
      };

      var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>(options);
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<ProductDto>("Error desconocido al obtener el producto.");
    }
  }

  public Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByTypeAsync(ProductType type)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<ProductDto>> SellProductAsync(SellProductDto sellProductDto)
  {
    throw new NotImplementedException();
  }

  public async Task<ApiResponse<ProductDto>> UpdateProductAsync(UpdateProductDto productDto)
  {
    try
    {
      var response = await _http.PutAsJsonAsync("api/v1/Product", productDto);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
        ? new ApiResponse<ProductDto>(errorResponse.message)
        : new ApiResponse<ProductDto>("Error al actualizar el producto.");
      }
      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
      };
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>(options);
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<ProductDto>("Error desconocido al actualizar el producto.");
    }
  }

  public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
  {
    try
    {
      var response = await _http.DeleteAsync($"api/v1/Product/{id}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
        ? new ApiResponse<bool>(errorResponse.message)
        : new ApiResponse<bool>("Error al eliminar el producto.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<bool>("Error desconocido al eliminar el producto.");
    }
  }
}