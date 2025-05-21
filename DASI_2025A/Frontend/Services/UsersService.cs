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

  public Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
  {
    throw new NotImplementedException();
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