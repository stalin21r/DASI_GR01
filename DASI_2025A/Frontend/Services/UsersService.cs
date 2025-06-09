using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Shared;

namespace Frontend;

public class UsersService : IUsersService
{
  private readonly HttpClient _http;
  public UsersService(HttpClient http)
  {
    _http = http;
  }

  public async Task<ApiResponse<UserDto>> CreateUserAsync(UserDto user)
  {
    try
    {
      var response = await _http.PostAsJsonAsync("api/v1/User", user);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<UserDto>(errorResponse.message)
          : new ApiResponse<UserDto>("Error al crear el usuario.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<UserDto>("Error desconocido al crear el usuario.");
    }
  }

  public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
  {
    try
    {
      var response = await _http.GetAsync("api/v1/User");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<IEnumerable<UserDto>>(errorResponse.message)
          : new ApiResponse<IEnumerable<UserDto>>("Error al obtener los usuarios.");
      }

      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserDto>>>();

      return result!;

    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<IEnumerable<UserDto>>("Error desconocido al obtener los usuarios.");
    }
  }

  public async Task<ApiResponse<UserDto>> GetUserByIdAsync(string id)
  {
    try
    {
      var response = await _http.GetAsync($"api/v1/User/{id}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<UserDto>(errorResponse.message)
          : new ApiResponse<UserDto>("Error al obtener el usuario.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<UserDto>("Error desconocido al obtener el usuario.");
    }
  }

  public async Task<ApiResponse<UserDto>> UpdateUserAsync(UserDto user)
  {
    try
    {
      var response = await _http.PutAsJsonAsync("api/v1/User", user);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<UserDto>(errorResponse.message)
          : new ApiResponse<UserDto>("Error al actualizar el usuario.");
      }
      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
      };
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>(options);
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<UserDto>("Error desconocido al actualizar el usuario.");
    }
  }

  public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
  {
    try
    {
      var response = await _http.DeleteAsync($"api/v1/User/{id}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<bool>(errorResponse.message)
          : new ApiResponse<bool>("Error al eliminar el usuario.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<bool>("Error desconocido al eliminar el usuario.");

    }
  }

  public async Task<ApiResponse<TopUpRequestResponseDto>> CreateTopUpRequestAsync(TopUpRequestCreateDto topUpRequestDto)
  {
    try
    {
      var response = await _http.PostAsJsonAsync("/api/v1/User/topuprequest/create", topUpRequestDto);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<TopUpRequestResponseDto>(errorResponse.message)
          : new ApiResponse<TopUpRequestResponseDto>("Error al crear la solicitud de recarga.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<TopUpRequestResponseDto>>();
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<TopUpRequestResponseDto>("Error desconocido al crear la solicitud de recarga.");
    }
  }

  public async Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetAllTopUpRequestsAsync()
  {
    try
    {
      var response = await _http.GetAsync("/api/v1/User/topuprequest");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<IEnumerable<TopUpRequestResponseDto>>(errorResponse.message)
          : new ApiResponse<IEnumerable<TopUpRequestResponseDto>>("Error al obtener las solicitudes de recarga.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<TopUpRequestResponseDto>>>();
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<IEnumerable<TopUpRequestResponseDto>>("Error desconocido al obtener las solicitudes de recarga.");
    }
  }

  public async Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetTopUpRequestsByUserIdAsync(string userId)
  {
    try
    {
      var response = await _http.GetAsync($"/api/v1/User/topuprequest/{userId}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<IEnumerable<TopUpRequestResponseDto>>(errorResponse.message)
          : new ApiResponse<IEnumerable<TopUpRequestResponseDto>>("Error al obtener las solicitudes de recarga.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<TopUpRequestResponseDto>>>();
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<IEnumerable<TopUpRequestResponseDto>>("Error desconocido al obtener las solicitudes de recarga.");
    }
  }

  public async Task<ApiResponse<TopUpRequestResponseDto>> AproveOrRejectTopUpAsync(TopUpRequestUpdateDto topUpRequestDto)
  {
    try
    {
      var response = await _http.PutAsJsonAsync("/api/v1/User/topuprequest/aprobe", topUpRequestDto);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<TopUpRequestResponseDto>(errorResponse.message)
          : new ApiResponse<TopUpRequestResponseDto>("Error al aprobar o rechazar la solicitud de recarga.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<TopUpRequestResponseDto>>();
      return result!;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new ApiResponse<TopUpRequestResponseDto>("Error desconocido al aprobar o rechazar la solicitud de recarga.");
    }
  }
}