using Backend.Data.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _cfg;

        public AuthService(UserManager<ApplicationUser> user, IConfiguration cfg)
        {
            _userManager = user; 
            _cfg = cfg;
        }

        // Registro de usuario
        public async Task<TokenDto> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Cedula = dto.Cedula
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) throw new InvalidOperationException(
                string.Join("; ", result.Errors.Select(e => e.Description)));

            // asignar rol por defecto
            await _userManager.AddToRoleAsync(user, "User");

            return await GenerateJwtAsync(user);
        }

        // Login de usuario
        public async Task<TokenDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            return await GenerateJwtAsync(user);
        }



        // ----------------- helpers privados -----------------
        private async Task<TokenDto> GenerateJwtAsync(ApplicationUser user)
        {
            var jwt = _cfg.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpireMins"]!)),
                signingCredentials: creds);

            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo
            };
        }
    }
}
