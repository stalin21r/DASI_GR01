using Shared;

namespace Frontend;

public interface IAuthService
{
  Task<ApiResponse<AuthDto>> LoginAsync(LoginDto loginDto);
  Task LogoutAsync();
  Task<ApiResponse<bool>> ActivateAccountAsync(ActivateUserDto activateUserDto);
  Task<ApiResponse<bool>> SendRecoverPassMailAsync(string email);
  Task<ApiResponse<bool>> ResetPasswordAsync(RecoverPassDto recoverPassDto);
}