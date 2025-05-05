using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthController"/> class with the specified authentication service.
    /// </summary>
    /// <param name="authService">The authentication service used to handle authentication operations.</param>
    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }


    /// <summary>
    ///     Inicia sesión con el usuario proporcionado.
    /// </summary>
    /// <param name="loginDto">Objeto que contiene el correo electrónico y la contraseña del usuario.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> que contiene un JWT si el inicio de sesión es exitoso.
    ///     Lanza una excepción <see cref="KeyNotFoundException"/> si el usuario no existe.
    ///     Lanza una excepción <see cref="UnauthorizedAccessException"/> si el usuario no está activo o si la contraseña es incorrecta.
    ///     En caso de error inesperado, lanza una excepción <see cref="Exception"/> con un mensaje que indica que hubo un error del servidor al iniciar sesión.
    /// </returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
      try
      {
        var response = await _authService.LoginAsync(loginDto);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (UnauthorizedAccessException ex)
      {
        return Unauthorized(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al iniciar sesión." });
      }
    }
  }
}
