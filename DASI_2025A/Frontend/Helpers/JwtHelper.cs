using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Frontend;

public static class JwtHelper
{
  public static string GetClaim(string token, string claimType)
  {
    var handler = new JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(token);
    return jwt.Claims.FirstOrDefault(c => c.Type == claimType)?.Value!;
  }
}
