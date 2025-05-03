using Shared.Dto;

namespace Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> RegisterAsync(RegisterDto dto);
        Task<TokenDto> LoginAsync(LoginDto dto);
    }
}
