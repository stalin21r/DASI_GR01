using Shared;

namespace Frontend;

public interface IUsersService
{
  Task<ApiResponse<UserDto>> CreateUserAsync(UserDto user);
  Task<ApiResponse<PagedResult<UserDto>>> GetAllUsersAsync(UserQueryParams query);
  Task<ApiResponse<UserDto>> GetUserByIdAsync(string id);
  Task<ApiResponse<UserDto>> UpdateUserAsync(UserDto user);
  Task<ApiResponse<bool>> DeleteUserAsync(string id);
  Task<ApiResponse<bool>> ChangePasswordAsync(ChangePassDto changePassDto);
  Task<ApiResponse<TopUpRequestResponseDto>> CreateTopUpRequestAsync(TopUpRequestCreateDto topUpRequestDto);
  Task<ApiResponse<PagedResult<TopUpRequestResponseDto>>> GetAllTopUpRequestsAsync(AdminTopUpRequestQueryParams query);
  Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetTopUpRequestsByUserIdAsync(string userId);
  Task<ApiResponse<TopUpRequestResponseDto>> AproveOrRejectTopUpAsync(TopUpRequestUpdateDto topUpRequestDto);
  Task<ApiResponse<UserTransactionsDto>> GetUserTransactionsAsync(string userId);
}