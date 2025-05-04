using Shared;

namespace Backend;
public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<ApiResponse<UserDto>> CreateAsync(UserDto userDto)
	{
		var result = await _userRepository.CreateAsync(userDto);
		if (result == null)
		{
			throw new BadHttpRequestException("Error al crear el nuevo usuario.");
		}
		ApiResponse<UserDto> response = new ApiResponse<UserDto>(
		  message: "Usuario creado exitosamente",
		  data: result,
		  totalRecords: 1
		);
		return response;
	}

	public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync()
	{
		var result = await _userRepository.GetAllAsync();
		if (result == null || !result.Any())
		{
			throw new KeyNotFoundException("No se encontraron Usuarios.");
		}
		ApiResponse<IEnumerable<UserDto>> response = new ApiResponse<IEnumerable<UserDto>>(
		  message: "Usuarios obtenidos exitosamente",
		  data: result,
		  totalRecords: result.Count()
		);
		return response;
	}

	public async Task<ApiResponse<UserDto>> GetAsync(string id)
	{
		var result = await _userRepository.GetAsync(id);
		if (result == null)
		{
			throw new KeyNotFoundException("No se encontraron Usuarios.");
		}
		ApiResponse<UserDto> response = new ApiResponse<UserDto>(
		  message: "Usuario obtenido exitosamente",
		  data: result,
		  totalRecords: 1
		);
		return response;
	}

	public async Task<ApiResponse<UserDto>> GetByEmailAsync(string email)
	{
		var result = await _userRepository.GetByEmailAsync(email);
		if (result == null)
		{
			throw new KeyNotFoundException("No se encontraron Usuarios.");
		}
		ApiResponse<UserDto> response = new ApiResponse<UserDto>(
		  message: "Usuario obtenido exitosamente",
		  data: result,
		  totalRecords: 1
		);
		return response;
	}

	public async Task<ApiResponse<UserDto>> UpdateAsync(UserDto userDto)
	{
		var result = await _userRepository.UpdateAsync(userDto);
		if (result == null)
		{
			throw new BadHttpRequestException("Error al actualizar el usuario.");
		}
		ApiResponse<UserDto> response = new ApiResponse<UserDto>(
		  message: "Usuario actualizado exitosamente",
		  data: result,
		  totalRecords: 1
		);
		return response;
	}

	public async Task<ApiResponse<bool>> DeleteAsync(string id)
	{
		var result = await _userRepository.DeleteAsync(id);
		if (!result)
		{
			throw new BadHttpRequestException("Error al eliminar el usuario.");
		}
		ApiResponse<bool> response = new ApiResponse<bool>(
		  message: "Usuario eliminado exitosamente",
		  data: result,
		  totalRecords: 0
		);
		return response;
	}
}