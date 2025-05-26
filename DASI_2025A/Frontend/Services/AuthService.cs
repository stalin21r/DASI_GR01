using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Security.Claims;
using Shared;

namespace Frontend;
public class AuthService : IAuthService
{
  private readonly HttpClient _http;
  private readonly ILocalStorageService _localStorage;
  public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
  {
    _http = httpClient;
    _localStorage = localStorage;
  }

  public async Task<ApiResponse<AuthDto>> LoginAsync(LoginDto loginDto)
  {
    try
    {
      var response = await _http.PostAsJsonAsync("api/v1/auth/login", loginDto);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<AuthDto>(errorResponse.message)
            : new ApiResponse<AuthDto>("Error al iniciar sesión.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthDto>>();
      Console.WriteLine(result?.data);
      if (result?.data is not null)
      {
        await _localStorage.SetItemAsync("token", result.data.token);
      }
      return result!;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new ApiResponse<AuthDto>("Error desconocido al iniciar sesión.");
    }
  }

  public Task LogoutAsync()
  {
    throw new NotImplementedException();
  }
}