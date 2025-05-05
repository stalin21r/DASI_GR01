using Shared;

namespace Backend;
public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;

  /// <summary>
  ///     Servicio para el manejo de usuarios.
  /// </summary>
  /// <param name="userRepository">Repositorio de usuarios.</param>
  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  /// <summary>
  ///     Crea un nuevo usuario.
  /// </summary>
  /// <param name="userDto">Objeto con los datos del usuario a crear.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{UserDto}"/> con los datos del usuario recién creado.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo crear el usuario.
  /// </returns>
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

  /// <summary>
  ///     Obtiene todos los usuarios.
  /// </summary>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{IEnumerable{UserDto}}"/> con todos los usuarios.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron usuarios.
  /// </returns>
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

  /// <summary>
  ///     Obtiene un usuario por su ID.
  /// </summary>
  /// <param name="id">El identificador del usuario.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{UserDto}"/> con el usuario solicitado.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron usuarios.
  /// </returns>
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

  /// <summary>
  ///     Obtiene un usuario por su correo electrónico.
  /// </summary>
  /// <param name="email">El correo electrónico del usuario.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{UserDto}"/> con el usuario solicitado.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron usuarios.
  /// </returns>
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

  /// <summary>
  ///     Actualiza un usuario existente.
  /// </summary>
  /// <param name="userDto">Objeto con los datos del usuario a actualizar.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{UserDto}"/> con los datos del usuario actualizado.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo actualizar el usuario.
  /// </returns>
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

  /// <summary>
  ///     Elimina un usuario existente.
  /// </summary>
  /// <param name="id">El identificador del usuario a eliminar.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{bool}"/> con el resultado de la operación.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo eliminar el usuario.
  /// </returns>
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
