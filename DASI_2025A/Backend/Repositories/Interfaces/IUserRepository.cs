using Shared;

namespace Backend;

public interface IUserRepository
{
  Task<(UserDto userDto, string token)> CreateAsync(UserDto userDto);
  Task<bool> ActivateUserAsync(ActivateUserDto activateUserDto);
  Task<PagedResult<UserDto>> GetAllAsync(UserQueryParams queryParams);
  Task<UserDto?> GetAsync(string id);
  Task<UserDto?> GetByEmailAsync(string email);
  Task<UserDto> UpdateAsync(UserDto userDto);
  Task<bool> DeleteAsync(string id);
  Task<bool> ChangePasswordAsync(string userId, ChangePassDto changePassDto);
  Task<(string Token, string FirstName)> RecoverPasswordAsync(string email);
  Task<bool> RecoverPasswordAsync(RecoverPassDto recoverPassDto);
  Task<bool> AssignRoleAsync(ApplicationUser userId, string roleName);
  Task<UserTransactionsDto> GetUserTransactionsAsync(string userId);
  Task<TopUpRequestResponseDto> CreateTopUpRequestAsync(TopUpRequestCreateDto topUpRequestDto);
  Task<TopUpRequestResponseDto> AproveOrRejectTopUpAsync(TopUpRequestUpdateDto topUpRequestDto);
  Task<PagedResult<TopUpRequestResponseDto>> GetTopUpRequestsAsync(AdminTopUpRequestQueryParams query);
  Task<IEnumerable<TopUpRequestResponseDto>> GetTopUpRequestsByUserIdAsync(string userId);
}
