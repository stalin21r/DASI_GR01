using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    /// <summary>
    ///     Controlador para la gestión de usuarios.
    /// </summary>
    /// <param name="userService">Servicio para la gestión de usuarios.</param>
    /// <param name="logger">Servicio de registro de la aplicación.</param>
    /// <param name="mailkitService">Servicio para el envío de correos electrónicos.</param>
    public UserController(IUserService userService, ILogger<UserController> logger, IMailkitService mailkitService)
    {
      _userService = userService;
      _logger = logger;
    }

    /// <summary>
    ///     Crea un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="userDto">Datos del usuario a crear.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 201 si el usuario fue creado exitosamente.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpPost]
    [Authorize(Policy = "SuperadminOnly")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
      _logger.LogInformation("Intentando crear un nuevo usuario.");
      try
      {
        var response = await _userService.CreateAsync(userDto);
        _logger.LogInformation("Usuario creado exitosamente.");
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al crear usuario: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al crear el usuario.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    /// <summary>
    ///     Activa un usuario en el sistema, previamente creado pero inactivo.
    /// </summary>
    /// <param name="activateUserDto">Objeto que contiene el correo electrónico y el token de activación.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si el usuario fue activado correctamente.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpPut]
    [Route("ActivateUser")]
    public async Task<IActionResult> ActivateUser([FromBody] ActivateUserDto activateUserDto)
    {
      _logger.LogInformation("Intentando activar el usuario {User}.", activateUserDto.Email);
      try
      {
        var response = await _userService.ActivateUserAsync(activateUserDto);
        _logger.LogInformation("Usuario {Usuario} activado exitosamente.", activateUserDto.Email);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Usuario no encontrado: {Message}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al activar usuario: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (InvalidOperationException ex)
      {
        _logger.LogWarning("Operación inválida al activar usuario: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al activar el usuario.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al activar el usuario." });
      }
    }

    /// <summary>
    ///     Obtiene la lista de todos los usuarios registrados en la aplicación.
    /// </summary>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con el resultado de la operación.
    ///     Retorna un código de estado 200 si se obtuvieron los usuarios correctamente.
    ///     Retorna un código de estado 404 si no se encontraron usuarios.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpGet]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryParams queryParams)
    {
      _logger.LogInformation("Solicitando listado de usuarios con filtros/paginación.");
      try
      {
        var response = await _userService.GetAllAsync(queryParams);
        _logger.LogInformation("Usuarios obtenidos correctamente.");
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron usuarios: {Message}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al obtener usuarios.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener los usuarios." });
      }
    }


    /// <summary>
    ///     Obtiene un usuario por su ID.
    /// </summary>
    /// <param name="id">El ID del usuario a obtener.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con el resultado de la operación.
    ///     Retorna un código de estado 200 si el usuario fue obtenido correctamente.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpGet]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
      _logger.LogInformation("Buscando usuario por ID: {Id}", id);
      try
      {
        var response = await _userService.GetAsync(id);
        _logger.LogInformation("Usuario encontrado con ID: {Id}", id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Usuario no encontrado con ID: {Id}. Mensaje: {Message}", id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al buscar usuario por ID: {Id}", id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener el usuario." });
      }
    }

    /// <summary>
    ///     Obtiene un usuario por su correo electrónico.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario a obtener.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con el resultado de la operación.
    ///     Retorna un código de estado 200 si el usuario fue obtenido correctamente.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpGet]
    [Route("email/{email}")]
    [Authorize]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
    {
      _logger.LogInformation("Buscando usuario por correo electrónico: {Email}", email);
      try
      {
        var response = await _userService.GetByEmailAsync(email);
        _logger.LogInformation("Usuario encontrado con correo: {Email}", email);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Usuario no encontrado con correo: {Email}. Mensaje: {Message}", email, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al buscar usuario por correo: {Email}", email);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener el usuario." });
      }
    }

    /// <summary>
    ///     Actualiza un usuario existente en el sistema.
    /// </summary>
    /// <param name="userDto">Objeto que contiene los datos actualizados del usuario.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si el usuario fue actualizado correctamente.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
    {
      _logger.LogInformation("Intentando actualizar usuario con ID: {Id}", userDto.Id);
      try
      {
        var response = await _userService.UpdateAsync(userDto);
        _logger.LogInformation("Usuario actualizado correctamente. ID: {Id}", userDto.Id);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al actualizar usuario: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Usuario no encontrado al actualizar. ID: {Id}. Mensaje: {Message}", userDto.Id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al actualizar usuario. ID: {Id}", userDto.Id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al actualizar el usuario." });
      }
    }

    /// <summary>
    ///     Elimina un usuario existente en el sistema.
    /// </summary>
    /// <param name="id">El ID del usuario a eliminar.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si el usuario fue eliminado correctamente.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Policy = "SuperadminOnly")]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
      _logger.LogInformation("Intentando eliminar usuario con ID: {Id}", id);
      try
      {
        var response = await _userService.DeleteAsync(id);
        _logger.LogInformation("Usuario eliminado correctamente. ID: {Id}", id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Usuario no encontrado al eliminar. ID: {Id}. Mensaje: {Message}", id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al eliminar usuario: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al eliminar usuario. ID: {Id}", id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al eliminar el usuario." });
      }
    }

    /// <summary>
    ///     Cambia la contraseña de un usuario autenticado.
    /// </summary>
    /// <param name="changePassDto">Objeto que contiene la contraseña actual y la nueva.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si la contraseña se cambió exitosamente.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>

    [HttpPatch]
    [Route("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassDto changePassDto)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Usuario {UserId} intentando cambiar la contraseña", userId);
        var response = await _userService.ChangePasswordAsync(userId!, changePassDto);
        _logger.LogInformation("Contraseña cambiada correctamente para el usuario con ID: {UserId}", userId);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud al cambiar la contraseña: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al cambiar la contraseña.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al cambiar la contraseña." });
      }
    }

    /// <summary>
    ///     Envía un correo electrónico para recuperar la contraseña de un usuario.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si el correo se envió correctamente.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpGet]
    [Route("SendRecoverMail/{email}")]
    public async Task<IActionResult> SendRecoverMail([FromRoute] string email)
    {
      try
      {
        var response = await _userService.RecoverPasswordAsync(email);
        _logger.LogInformation("Se envió correo de recuperación al correo {Email}", email);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Error al enviar correo de recuperación: {Message}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud para enviar correo de recuperación: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al enviar correo de recuperación.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al recuperar la contraseña." });
      }
    }

    /// <summary>
    ///     Restablece la contraseña de un usuario, previamente se debe enviar un correo
    ///     de recuperación.
    /// </summary>
    /// <param name="recoverPassDto">Objeto que contiene la contraseña actual y la nueva.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si la contraseña se restableció exitosamente.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpPut]
    [Route("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] RecoverPassDto recoverPassDto)
    {
      try
      {
        _logger.LogInformation("Intentando restablecer la contraseña del usuario {Email}", recoverPassDto.Email);
        var response = await _userService.RecoverPasswordAsync(recoverPassDto);
        _logger.LogInformation("Contraseña del usuario {Email} restablecida correctamente.", recoverPassDto.Email);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Error al restablecer la contraseña: {Message}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud al restablecer la contraseña: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al restablecer la contraseña.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al restablecer la contraseña." });
      }
    }

    /// <summary>
    ///     Obtiene las transacciones de un usuario.
    /// </summary>
    /// <param name="id">El ID del usuario cuyas transacciones se desean obtener.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si las transacciones se obtuvieron correctamente.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpGet]
    [Route("transactions/{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserTransactions([FromRoute] string id)
    {
      _logger.LogInformation("Obteniendo transacciones del usuario con ID: {Id}", id);
      try
      {
        var response = await _userService.GetUserTransactionsAsync(id);
        _logger.LogInformation("Transacciones obtenidas correctamente para el usuario con ID: {Id}", id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron transacciones para el usuario con ID: {Id}. Mensaje: {Message}", id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al obtener transacciones del usuario con ID: {Id}", id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener las transacciones del usuario." });
      }
    }

    /// <summary>
    ///     Crea una solicitud de recarga para el usuario especificado.
    /// </summary>
    /// <param name="topUpRequestDto">El objeto que contiene la información necesaria para crear la solicitud de recarga.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 201 si la solicitud de recarga se creó correctamente.
    ///     Retorna un código de estado 404 si no se encontró el usuario.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpPost]
    [Route("topuprequest/create")]
    [Authorize]
    public async Task<IActionResult> CreateTopUpRequest([FromBody] TopUpRequestCreateDto topUpRequestDto)
    {
      _logger.LogInformation("Creando solicitud de recarga para el usuario con ID: {UserId}", topUpRequestDto.TargetUserId);
      try
      {
        var response = await _userService.CreateTopUpRequestAsync(topUpRequestDto);
        _logger.LogInformation("Solicitud de recarga creada exitosamente para el usuario con ID: {UserId}", topUpRequestDto);
        return Created(string.Empty, response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Usuario no encontrado al crear solicitud de recarga. ID: {UserId}. Mensaje: {Message}", topUpRequestDto, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al crear recarga: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al crear solicitud de recarga para el usuario con ID: {UserId}", topUpRequestDto.TargetUserId);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear solicitud." });
      }
    }

    /// <summary>
    ///     Aprobar o rechazar una solicitud de recarga.
    /// </summary>
    /// <param name="topUpRequestDto">El objeto con los datos de la solicitud de recarga.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si la solicitud se procesó correctamente.
    ///     Retorna un código de estado 404 si no se encontró la solicitud de recarga.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpPut]
    [Route("topuprequest/aprobe")]
    [Authorize]
    public async Task<IActionResult> AproveOrRejectTopUp([FromBody] TopUpRequestUpdateDto topUpRequestDto)
    {
      _logger.LogInformation("Aprobando o rechazando solicitud de recarga con ID: {RequestId}", topUpRequestDto.Id);
      try
      {
        var response = await _userService.AproveOrRejectTopUpAsync(topUpRequestDto);
        _logger.LogInformation("Solicitud de recarga procesada correctamente. ID: {RequestId}", topUpRequestDto.Id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Solicitud de recarga no encontrada. ID: {RequestId}. Mensaje: {Message}", topUpRequestDto.Id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al aprobar/rechazar recarga: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al aprobar/rechazar solicitud de recarga. ID: {RequestId}", topUpRequestDto.Id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al aprobar solicitud.\n" + ex.Message });
      }
    }

    /// <summary>
    ///     Obtiene todas las solicitudes de recarga.
    /// </summary>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si las solicitudes de recarga se obtuvieron correctamente.
    ///     Retorna un código de estado 404 si no se encontraron solicitudes de recarga.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpGet]
    [Route("topuprequest")]
    [Authorize(Policy = "SuperadminOnly")]
    public async Task<IActionResult> GetAllTopUpRequests([FromQuery] AdminTopUpRequestQueryParams query)
    {
      _logger.LogInformation("Obteniendo todas las solicitudes de recarga.");
      try
      {
        var response = await _userService.GetTopUpRequestsAsync(query);
        _logger.LogInformation("Solicitudes de recarga obtenidas correctamente.");
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron solicitudes de recarga: {Message}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al obtener recargas: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al obtener solicitudes de recarga.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener recargas.\n" + ex.Message });
      }
    }


    /// <summary>
    ///     Obtiene las solicitudes de recarga de saldo para un usuario específico por su ID.
    /// </summary>
    /// <param name="id">El ID del usuario cuyas solicitudes de recarga se desean obtener.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> indicando el resultado de la operación.
    ///     Retorna un código de estado 200 si las solicitudes de recarga se obtuvieron correctamente.
    ///     Retorna un código de estado 404 si no se encontraron solicitudes de recarga para el usuario.
    ///     Retorna un código de estado 400 si la solicitud es inválida.
    ///     Retorna un código de estado 500 si ocurre un error inesperado en el servidor.
    /// </returns>
    [HttpGet]
    [Route("topuprequest/{id}")]
    [Authorize]
    public async Task<IActionResult> GetTopUpRequestById([FromRoute] string id)
    {
      _logger.LogInformation("Obteniendo solicitudes de recarga para el usuario con ID: {Id}", id);
      try
      {
        var response = await _userService.GetTopUpRequestsByUserIdAsync(id);
        _logger.LogInformation("Solicitudes de recarga obtenidas correctamente para el usuario con ID: {Id}", id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron solicitudes de recarga para el usuario con ID: {Id}. Mensaje: {Message}", id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud inválida al obtener recargas para el usuario: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al obtener recargas para el usuario con ID: {Id}", id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener recargas." });
      }
    }
  }
}
