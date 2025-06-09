using Shared;

namespace Frontend;

public interface IUsersService
{
  Task<ApiResponse<UserDto>> CreateUserAsync(UserDto user);
  Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
  Task<ApiResponse<UserDto>> GetUserByIdAsync(string id);
  Task<ApiResponse<UserDto>> UpdateUserAsync(UserDto user);
  Task<ApiResponse<bool>> DeleteUserAsync(string id);
  Task<ApiResponse<TopUpRequestResponseDto>> CreateTopUpRequestAsync(TopUpRequestCreateDto topUpRequestDto);
  Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetAllTopUpRequestsAsync();
  Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetTopUpRequestsByUserIdAsync(string userId);
  Task<ApiResponse<TopUpRequestResponseDto>> AproveOrRejectTopUpAsync(TopUpRequestUpdateDto topUpRequestDto);

}