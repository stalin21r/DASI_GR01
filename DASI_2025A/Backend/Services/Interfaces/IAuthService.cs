using Shared;
namespace Backend;

public interface IAuthService
{
  Task<ApiResponse<AuthDto>> LoginAsync(LoginDto loginDto);
}
