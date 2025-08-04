using Shared;

namespace Backend;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;
  private readonly IMailkitService _mailkitService;

  /// <summary>
  ///     Servicio para el manejo de usuarios.
  /// </summary>
  /// <param name="userRepository">Repositorio de usuarios.</param>
  public UserService(IUserRepository userRepository, IMailkitService mailkitService)
  {
    _userRepository = userRepository;
    _mailkitService = mailkitService;
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
    if (result.userDto == null || result.token == null)
    {
      throw new BadHttpRequestException("Error al crear el nuevo usuario.");
    }
    var mailResult = await _mailkitService.SendActivationMail(result.userDto.Email, result.userDto.FirstName, result.token);
    if (!mailResult)
    {
      throw new BadHttpRequestException("Error al enviar el correo de activación.");
    }
    ApiResponse<UserDto> response = new ApiResponse<UserDto>(
      message: $"Usuario creado exitosamente, {(mailResult ? "correo enviado" : "error: correo no enviado")}",
      data: result.userDto,
      totalRecords: 1
    );
    return response;
  }

  /// <summary>
  ///     Activa un usuario existente.
  /// </summary>
  /// <param name="activateUserDto">Objeto con los datos del usuario a activar.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{bool}"/> con el resultado de la operación.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo activar el usuario.
  /// </returns>
  /// <remarks>
  ///     Verifica que se hayan aceptado los términos y condiciones, y la política de privacidad.
  ///     Verifica que las contraseñas coincidan.
  /// </remarks>
  public async Task<ApiResponse<bool>> ActivateUserAsync(ActivateUserDto activateUserDto)
  {
    if (!activateUserDto.TermsConditions) throw new BadHttpRequestException("Debe aceptar los términos y condiciones.");
    if (!activateUserDto.PrivacyPolicy) throw new BadHttpRequestException("Debe aceptar la política de privacidad.");
    if (activateUserDto.NewPassword != activateUserDto.ConfirmNewPassword) throw new BadHttpRequestException("Las contraseñas no coinciden.");
    var result = await _userRepository.ActivateUserAsync(activateUserDto);
    if (!result) throw new BadHttpRequestException("Error al activar el usuario.");
    ApiResponse<bool> response = new ApiResponse<bool>(
      message: "Usuario activado exitosamente",
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
  public async Task<ApiResponse<PagedResult<UserDto>>> GetAllAsync(UserQueryParams queryParams)
  {
    var result = await _userRepository.GetAllAsync(queryParams);
    if (result.Items == null || !result.Items.Any())
    {
      throw new KeyNotFoundException("No se encontraron Usuarios.");
    }

    return new ApiResponse<PagedResult<UserDto>>(
      message: "Usuarios obtenidos exitosamente",
      data: result,
      totalRecords: result.TotalItems
    );
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

  /// <summary>
  ///     Cambia la contraseña de un usuario existente.
  /// </summary>
  /// <param name="userId">El identificador del usuario.</param>
  /// <param name="changePassDto">Objeto con los datos de la contraseña actual y la nueva.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{bool}"/> con el resultado de la operación.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo cambiar la contraseña.
  /// </returns>
  public async Task<ApiResponse<bool>> ChangePasswordAsync(string userId, ChangePassDto changePassDto)
  {
    var result = await _userRepository.ChangePasswordAsync(userId, changePassDto);
    if (!result)
    {
      throw new BadHttpRequestException("Error al cambiar la contraseña.");
    }
    ApiResponse<bool> response = new ApiResponse<bool>(
      message: "Contraseña cambiada exitosamente",
      data: result,
      totalRecords: 0
    );
    return response;
  }

  /// <summary>
  ///     Envía un correo de recuperación de contraseña a un usuario registrado.
  /// </summary>
  /// <param name="email">El correo electrónico del usuario.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{bool}"/> indicando si el correo de recuperación fue enviado exitosamente.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si ocurre un error al enviar el correo de recuperación.
  /// </returns>
  /// <remarks>
  ///     Verifica que el token y el primer nombre del usuario no sean nulos antes de enviar el correo.
  /// </remarks>
  public async Task<ApiResponse<bool>> RecoverPasswordAsync(string email)
  {
    var result = await _userRepository.RecoverPasswordAsync(email);
    if (result.FirstName == null || result.Token == null) throw new BadHttpRequestException("Error al enviar el correo de recuperación de contraseña.");
    var sendPasswordRecoveryMail = await _mailkitService.SendPasswordRecoveryMail(email, result.FirstName, result.Token);
    if (!sendPasswordRecoveryMail) throw new BadHttpRequestException("Error al enviar el correo de recuperación de contraseña.");
    ApiResponse<bool> response = new ApiResponse<bool>(
      message: "Correo enviado exitosamente",
      data: true,
      totalRecords: 0
    );
    return response;
  }

  /// <summary>
  ///     Resetea la contraseña de un usuario al que se le ha enviado un correo de recuperación.
  /// </summary>
  /// <param name="recoverPassDto">Objeto que contiene el correo electrónico del usuario, el token de recuperación y la nueva contraseña.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{bool}"/> indicando si la operación de reseteo de contraseña fue exitosa.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si el usuario no esta activo.
  /// </returns>
  /// <remarks>
  ///     Verifica que las contraseñas coincidan antes de cambiar la contraseña.
  /// </remarks>
  public async Task<ApiResponse<bool>> RecoverPasswordAsync(RecoverPassDto recoverPassDto)
  {
    if (recoverPassDto.NewPassword != recoverPassDto.ConfirmNewPassword) throw new BadHttpRequestException("Las contraseñas no coinciden.");
    var result = await _userRepository.RecoverPasswordAsync(recoverPassDto);
    if (!result)
    {
      throw new BadHttpRequestException("Error al cambiar la contraseña.");
    }
    ApiResponse<bool> response = new ApiResponse<bool>(
      message: "Contraseña cambiada exitosamente",
      data: result,
      totalRecords: 0
    );
    return response;
  }

  /// <summary>
  ///     Obtiene las transacciones de un usuario específico.
  /// </summary>
  /// <param name="userId">El identificador del usuario cuyas transacciones se desean obtener.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{UserTransactionsDto}"/> que contiene las transacciones del usuario.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encuentra el usuario.
  /// </returns>
  public async Task<ApiResponse<UserTransactionsDto>> GetUserTransactionsAsync(string userId)
  {
    var result = await _userRepository.GetUserTransactionsAsync(userId);
    if (result == null)
    {
      throw new KeyNotFoundException("Usuario no encontrado.");
    }
    ApiResponse<UserTransactionsDto> response = new ApiResponse<UserTransactionsDto>(
      message: "Transacciones obtenidas exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  /// <summary>
  ///     Crea una solicitud de recarga de saldo para un usuario.
  /// </summary>
  /// <param name="topUpRequestDto">Objeto con los datos de la solicitud de recarga.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{TopUpRequestResponseDto}"/> que contiene la solicitud de recarga recién creada.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo insertar la solicitud de recarga.
  /// </returns>
  public async Task<ApiResponse<TopUpRequestResponseDto>> CreateTopUpRequestAsync(TopUpRequestCreateDto topUpRequestDto)
  {
    var result = await _userRepository.CreateTopUpRequestAsync(topUpRequestDto);
    if (result == null)
    {
      throw new BadHttpRequestException("Error al crear la solicitud de recarga.");
    }
    ApiResponse<TopUpRequestResponseDto> response = new ApiResponse<TopUpRequestResponseDto>(
      message: "Solicitud de recarga creada exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  /// <summary>
  ///     Actualiza el estado de una solicitud de recarga a APROBADO o RECHAZADO.
  /// </summary>
  /// <param name="topUpRequestDto">Objeto con los datos de la solicitud de recarga que se va a actualizar.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{TopUpRequestResponseDto}"/> que contiene la solicitud de recarga actualizada.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo actualizar la solicitud de recarga.
  /// </returns>
  public async Task<ApiResponse<TopUpRequestResponseDto>> AproveOrRejectTopUpAsync(TopUpRequestUpdateDto topUpRequestDto)
  {
    var result = await _userRepository.AproveOrRejectTopUpAsync(topUpRequestDto);
    if (result == null)
    {
      throw new BadHttpRequestException("Error al actualizar la solicitud de recarga.");
    }
    ApiResponse<TopUpRequestResponseDto> response = new ApiResponse<TopUpRequestResponseDto>(
      message: "Solicitud de recarga actualizada exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  /// <summary>
  ///     Obtiene todas las solicitudes de recarga de saldo.
  /// </summary>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{IEnumerable{TopUpRequestResponseDto}}"/> con la lista de solicitudes de recarga.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron solicitudes de recarga.
  /// </returns>
  public async Task<ApiResponse<PagedResult<TopUpRequestResponseDto>>> GetTopUpRequestsAsync(AdminTopUpRequestQueryParams query)
  {
    var result = await _userRepository.GetTopUpRequestsAsync(query);
    if (result == null || !result.Items.Any())
    {
      throw new KeyNotFoundException("No se encontraron solicitudes de recarga.");
    }

    return new ApiResponse<PagedResult<TopUpRequestResponseDto>>(
      message: "Solicitudes de recarga obtenidas exitosamente",
      data: result,
      totalRecords: result.TotalItems
    );
  }

  /// <summary>
  ///     Obtiene las solicitudes de recarga de saldo del usuario con el identificador especificado.
  /// </summary>
  /// <param name="userId">El identificador del usuario cuyas solicitudes de recarga se desean obtener.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{IEnumerable{TopUpRequestResponseDto}}"/> con la lista de solicitudes de recarga del usuario.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron solicitudes de recarga para el usuario.
  /// </returns>
  public async Task<ApiResponse<IEnumerable<TopUpRequestResponseDto>>> GetTopUpRequestsByUserIdAsync(string userId)
  {
    var result = await _userRepository.GetTopUpRequestsByUserIdAsync(userId);
    if (result == null)
    {
      throw new KeyNotFoundException("No se encontraron solicitudes de recarga.");
    }
    ApiResponse<IEnumerable<TopUpRequestResponseDto>> response = new ApiResponse<IEnumerable<TopUpRequestResponseDto>>(
      message: "Solicitudes de recarga obtenidas exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }

}
