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
    ///     Controlador para el manejo de usuarios.
    /// </summary>
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
      _userService = userService;
      _logger = logger;
    }

    /// <summary>
    ///     Crea un nuevo usuario.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "SuperadminOnly")]
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
    ///     Obtiene todos los usuarios.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> GetAllUsers()
    {
      _logger.LogInformation("Solicitando listado de todos los usuarios.");
      try
      {
        var response = await _userService.GetAllAsync();
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
    ///     Actualiza la información de un usuario existente.
    /// </summary>
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
    ///     Elimina un usuario.
    /// </summary>
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

    [HttpGet]
    [Route("topuprequest")]
    [Authorize(Policy = "SuperadminOnly")]
    public async Task<IActionResult> GetAllTopUpRequests()
    {
      _logger.LogInformation("Obteniendo todas las solicitudes de recarga.");
      try
      {
        var response = await _userService.GetTopUpRequestsAsync();
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
