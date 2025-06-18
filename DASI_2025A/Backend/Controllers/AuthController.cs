using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    ///     Inicializa una nueva instancia de la clase <see cref="AuthController"/> con el servicio de autenticación especificado.
    /// </summary>
    /// <param name="authService">El servicio de autenticación utilizado para manejar operaciones de autenticación.</param>
    /// <param name="logger">El logger para registrar información, advertencias y errores.</param>
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
      _authService = authService;
      _logger = logger;
    }

    /// <summary>
    ///     Inicia sesión con el usuario proporcionado.
    /// </summary>
    /// <param name="loginDto">Objeto que contiene el correo electrónico y la contraseña del usuario.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> que contiene un JWT si el inicio de sesión es exitoso.
    ///     Lanza una excepción <see cref="KeyNotFoundException"/> si el usuario no existe.
    ///     Lanza una excepción <see cref="UnauthorizedAccessException"/> si el usuario no está activo o si la contraseña es incorrecta.
    ///     En caso de error inesperado, lanza una excepción <see cref="Exception"/> con un mensaje que indica que hubo un error del servidor al iniciar sesión.
    /// </returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
      _logger.LogInformation("Intento de inicio de sesión para el usuario: {Email}", loginDto.Email);
      try
      {
        var response = await _authService.LoginAsync(loginDto);
        _logger.LogInformation("Inicio de sesión exitoso para el usuario: {Email}", loginDto.Email);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("Intento de inicio de sesión fallido: usuario no encontrado ({Email}).", loginDto.Email);
        return NotFound(new { message = ex.Message });
      }
      catch (UnauthorizedAccessException ex)
      {
        _logger.LogWarning("Intento de inicio de sesión no autorizado para el usuario: {Email}. Motivo: {Reason}", loginDto.Email, ex.Message);
        return Unauthorized(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al intentar iniciar sesión para el usuario: {Email}", loginDto.Email);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al iniciar sesión." });
      }
    }
  }
}
