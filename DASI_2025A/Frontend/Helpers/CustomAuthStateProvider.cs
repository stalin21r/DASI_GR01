using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Frontend;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
  private readonly ILocalStorageService _localStorage;

  public CustomAuthStateProvider(ILocalStorageService localStorage)
  {
    _localStorage = localStorage;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var token = await _localStorage.GetItemAsStringAsync("token");

    if (string.IsNullOrWhiteSpace(token))
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    var handler = new JwtSecurityTokenHandler();
    JwtSecurityToken jwt;

    try
    {
      jwt = handler.ReadJwtToken(token);
    }
    catch
    {
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    // Verificar expiración
    var exp = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
    if (exp is not null && long.TryParse(exp, out var expUnix))
    {
      var expiration = DateTimeOffset.FromUnixTimeSeconds(expUnix);
      if (expiration < DateTime.UtcNow)
      {
        await _localStorage.RemoveItemAsync("token");
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
      }
    }

    var identity = new ClaimsIdentity(jwt.Claims, "jwt");
    var user = new ClaimsPrincipal(identity);
    return new AuthenticationState(user);
  }

  public void NotifyUserAuthentication(string token)
  {
    var handler = new JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(token);
    var identity = new ClaimsIdentity(jwt.Claims, "jwt");
    var user = new ClaimsPrincipal(identity);
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
  }

  public void NotifyUserLogout()
  {
    var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
  }
}
