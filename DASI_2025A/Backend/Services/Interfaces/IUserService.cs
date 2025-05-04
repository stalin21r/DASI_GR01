using Shared;

namespace Backend;
public interface IUserService
{
	Task<ApiResponse<UserDto>> CreateAsync(UserDto userDto);
	Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync();
	Task<ApiResponse<UserDto>> GetAsync(string id);
	Task<ApiResponse<UserDto>> GetByEmailAsync(string email);
	Task<ApiResponse<UserDto>> UpdateAsync(UserDto userDto);
	Task<ApiResponse<bool>> DeleteAsync(string id);
}