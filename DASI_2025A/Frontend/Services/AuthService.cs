using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Security.Claims;
using Shared;

namespace Frontend;

public class AuthService : IAuthService
{
  private readonly HttpClient _http;
  private readonly ILocalStorageService _localStorage;
  private readonly CustomAuthStateProvider _authStateProvider;
  public AuthService(HttpClient httpClient, ILocalStorageService localStorage, CustomAuthStateProvider authStateProvider)
  {
    _http = httpClient;
    _localStorage = localStorage;
    _authStateProvider = authStateProvider;
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
        _authStateProvider.NotifyUserAuthentication(result.data.token);
      }
      return result!;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new ApiResponse<AuthDto>("Error desconocido al iniciar sesión.");
    }
  }

  public async Task LogoutAsync()
  {
    await _localStorage.RemoveItemAsync("token");
    _authStateProvider.NotifyUserLogout();
  }

  public async Task<ApiResponse<bool>> ActivateAccountAsync(ActivateUserDto activateUserDto)
  {
    try
    {
      var response = await _http.PutAsJsonAsync("api/v1/User/ActivateUser", activateUserDto);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<bool>(errorResponse.message)
            : new ApiResponse<bool>("Error al activar el usuario.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
      return result!;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new ApiResponse<bool>("Error desconocido al activar el usuario.");
    }
  }

  public async Task<ApiResponse<bool>> SendRecoverPassMailAsync(string email)
  {
    try
    {
      var response = await _http.GetAsync($"api/v1/User/SendRecoverMail/{email}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<bool>(errorResponse.message)
            : new ApiResponse<bool>("Error al enviar correo de restablecimiento.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
      return result!;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new ApiResponse<bool>("Error desconocido al enviar correo de restablecimiento.");
    }
  }

  public async Task<ApiResponse<bool>> ResetPasswordAsync(RecoverPassDto recoverPassDto)
  {
    try
    {
      var response = await _http.PutAsJsonAsync("api/v1/User/ResetPassword", recoverPassDto);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
            ? new ApiResponse<bool>(errorResponse.message)
            : new ApiResponse<bool>("Error al restablecer la contraseña.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
      return result!;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new ApiResponse<bool>("Error desconocido al restablecer la contraseña.");
    }
  }
}