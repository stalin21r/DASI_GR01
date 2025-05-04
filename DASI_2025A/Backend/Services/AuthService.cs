using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shared;

namespace Backend;
public class AuthService : IAuthService
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IConfiguration _configuration;
  public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
  {
    _userManager = userManager;
    _configuration = configuration;
  }
  public async Task<ApiResponse<AuthDto>> LoginAsync(LoginDto loginDto)
  {
    var user = await _userManager.FindByEmailAsync(loginDto.Email);
    if (user == null)
    {
      throw new KeyNotFoundException("Usuario no registrado.");
    }
    if (!user.Active)
    {
      throw new UnauthorizedAccessException("El usuario no esta activo.");
    }
    var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
    if (!checkPassword)
    {
      throw new UnauthorizedAccessException("Contraseña incorrecta.");
    }
    var token = await GenerateJwtToken(user);
    AuthDto authDto = new AuthDto
    {
      token = token
    };
    ApiResponse<AuthDto> response = new ApiResponse<AuthDto>(
      message: "Inicio de sesión exitoso.",
      data: authDto,
      totalRecords: 1
    );
    return response;
  }

  private async Task<string> GenerateJwtToken(ApplicationUser user)
  {
    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
    new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
    };

    var jwtSettings = _configuration.GetSection("Jwt");
    var roles = await _userManager.GetRolesAsync(user);
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
      jwtSettings["Issuer"],
      jwtSettings["Audience"],
      claims,
      expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
      signingCredentials: credentials
    );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }

}
