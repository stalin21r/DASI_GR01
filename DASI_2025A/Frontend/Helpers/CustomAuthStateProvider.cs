using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Frontend;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
  private readonly ILocalStorageService _localStorage;
  private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

  public CustomAuthStateProvider(ILocalStorageService localStorage)
  {
    _localStorage = localStorage;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    try
    {
      var token = await _localStorage.GetItemAsStringAsync("token");

      if (string.IsNullOrWhiteSpace(token))
        return new AuthenticationState(_anonymous);

      var claims = ParseClaimsFromJwt(token);

      if (claims == null || !claims.Any())
        return new AuthenticationState(_anonymous);

      // Verificar si el token ha expirado
      var expiryClaim = claims.FirstOrDefault(c => c.Type == "exp");
      if (expiryClaim != null && long.TryParse(expiryClaim.Value, out var exp))
      {
        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(exp);
        if (expiryDate <= DateTimeOffset.UtcNow)
        {
          await _localStorage.RemoveItemAsync("token");
          return new AuthenticationState(_anonymous);
        }
      }

      // Crear la identidad del usuario con los claims del token
      var identity = new ClaimsIdentity(claims, "jwt");
      var user = new ClaimsPrincipal(identity);

      return new AuthenticationState(user);
    }
    catch
    {
      await _localStorage.RemoveItemAsync("token");
      return new AuthenticationState(_anonymous);
    }
  }

  public void NotifyUserAuthentication(string token)
  {
    var claims = ParseClaimsFromJwt(token);
    if (claims != null && claims.Any())
    {
      var identity = new ClaimsIdentity(claims, "jwt");
      var user = new ClaimsPrincipal(identity);
      NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
  }

  public void NotifyUserLogout()
  {
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
  }

  private IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
  {
    try
    {
      var handler = new JwtSecurityTokenHandler();
      var token = handler.ReadJwtToken(jwt);

      var claims = new List<Claim>();

      // Agregar todos los claims del token
      claims.AddRange(token.Claims);

      // Asegurar que tenemos el claim de rol en el formato correcto
      var roleClaims = token.Claims.Where(c => c.Type == "role" || c.Type == ClaimTypes.Role).ToList();

      // Si tenemos claims de rol con tipo "role", convertirlos a ClaimTypes.Role
      foreach (var roleClaim in roleClaims.Where(c => c.Type == "role"))
      {
        if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == roleClaim.Value))
        {
          claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
        }
      }

      return claims;
    }
    catch
    {
      return null;
    }
  }

  // Método útil para obtener el usuario actual
  public async Task<ClaimsPrincipal> GetCurrentUserAsync()
  {
    var state = await GetAuthenticationStateAsync();
    return state.User;
  }

  // Método para verificar si el usuario tiene un rol específico
  public async Task<bool> IsInRoleAsync(string role)
  {
    var user = await GetCurrentUserAsync();
    return user.IsInRole(role);
  }
}