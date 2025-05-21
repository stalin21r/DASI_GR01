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

  public Task<ApiResponse<UserDto>> CreateUserAsync(UserDto user)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<bool>> DeleteUserAsync(int id)
  {
    throw new NotImplementedException();
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

  public Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<UserDto>> UpdateUserAsync(UpdateProductDto productDto)
  {
    throw new NotImplementedException();
  }
}