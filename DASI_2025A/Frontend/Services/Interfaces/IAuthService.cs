using Shared;

namespace Frontend;
public interface IAuthService
{
  Task<ApiResponse<AuthDto>> LoginAsync(LoginDto loginDto);
  Task LogoutAsync();
}