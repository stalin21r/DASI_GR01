using Shared;

namespace Frontend;

public interface IUsersService
{
  Task<ApiResponse<UserDto>> CreateUserAsync(UserDto user);
  Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
  Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
  Task<ApiResponse<UserDto>> UpdateUserAsync(UpdateProductDto productDto);
  Task<ApiResponse<bool>> DeleteUserAsync(int id);

}