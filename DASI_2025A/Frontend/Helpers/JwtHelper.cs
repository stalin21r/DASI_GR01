using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Frontend;

public static class JwtHelper
{
  public static string GetClaim(string token, string claimType)
  {
    try
    {
      var handler = new JwtSecurityTokenHandler();
      var jwt = handler.ReadJwtToken(token);

      var claim = jwt.Claims.FirstOrDefault(c => c.Type == claimType);
      return claim?.Value ?? string.Empty;
    }
    catch
    {
      return string.Empty;
    }
  }

  public static IEnumerable<string> GetRoles(string token)
  {
    try
    {
      var handler = new JwtSecurityTokenHandler();
      var jwt = handler.ReadJwtToken(token);

      return jwt.Claims
          .Where(c => c.Type == "role" || c.Type == ClaimTypes.Role)
          .Select(c => c.Value)
          .ToList();
    }
    catch
    {
      return Enumerable.Empty<string>();
    }
  }

  public static bool IsTokenExpired(string token)
  {
    try
    {
      var handler = new JwtSecurityTokenHandler();
      var jwt = handler.ReadJwtToken(token);

      var exp = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
      if (exp != null && long.TryParse(exp, out var expUnix))
      {
        var expiration = DateTimeOffset.FromUnixTimeSeconds(expUnix);
        return expiration <= DateTimeOffset.UtcNow;
      }

      return true; // Si no tiene exp claim, considerarlo expirado
    }
    catch
    {
      return true; // Si hay error, considerarlo expirado
    }
  }

  public static DateTime? GetTokenExpiration(string token)
  {
    try
    {
      var handler = new JwtSecurityTokenHandler();
      var jwt = handler.ReadJwtToken(token);

      var exp = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
      if (exp != null && long.TryParse(exp, out var expUnix))
      {
        return DateTimeOffset.FromUnixTimeSeconds(expUnix).DateTime;
      }

      return null;
    }
    catch
    {
      return null;
    }
  }
}