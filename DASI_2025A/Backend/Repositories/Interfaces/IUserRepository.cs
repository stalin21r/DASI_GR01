using Backend.Data.Models;
using Shared;

namespace Backend;
public interface IUserRepository
{
	Task<UserDto> CreateAsync(UserDto userDto);
	Task<IEnumerable<UserDto>> GetAllAsync();
	Task<UserDto?> GetAsync(string id);
	Task<UserDto?> GetByEmailAsync(string email);
	Task<UserDto> UpdateAsync(UserDto userDto);
	Task<bool> DeleteAsync(string id);
	Task<bool> AssignRoleAsync(ApplicationUser userId, string roleName);
}