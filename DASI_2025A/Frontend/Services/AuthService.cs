using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Shared;

namespace Frontend;
public class AuthService : IAuthService
{
  private readonly HttpClient _http;
  private readonly IJSRuntime _js;
  public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
  {
    _http = httpClient;
    _js = jsRuntime;
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
        await _js.InvokeVoidAsync("localStorage.setItem", "token", result.data.token);
      }
      return result!;
    }
    catch
    {
      return new ApiResponse<AuthDto>("Error desconocido al iniciar sesión.");
    }
  }

  public Task LogoutAsync()
  {
    throw new NotImplementedException();
  }
}