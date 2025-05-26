using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    /// <summary>
    ///     Controlador para el manejo de usuarios.
    /// </summary>
    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    /// <summary>
    ///     Crea un nuevo usuario.
    /// </summary>
    /// <param name="userDto">Los datos del usuario a crear.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status201Created"/> si el usuario se crea correctamente.
    ///     En caso de error, retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status400BadRequest"/> o <see cref="StatusCodes.Status500InternalServerError"/>.
    /// </returns>

    [HttpPost]
    [Authorize(Policy = "SuperadminOnly")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
      try
      {
        var response = await _userService.CreateAsync(userDto);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }


    /// <summary>
    ///     Obtiene todos los usuarios.
    /// </summary>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status200OK"/> si los usuarios se obtienen correctamente.
    ///     En caso de error, retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status404NotFound"/> si no se encuentran los usuarios.
    ///     Si ocurre un error inesperado, retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status500InternalServerError"/>.
    /// </returns>
    [HttpGet]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> GetAllUsers()
    {
      try
      {
        var response = await _userService.GetAllAsync();
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message }); ;
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    /// <summary>
    ///     Obtiene un usuario por su ID.
    /// </summary>
    /// <param name="id">El identificador del usuario.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con código 200 OK si el usuario se obtiene correctamente.
    ///     Retorna un <see cref="IActionResult"/> con código 404 Not Found si el usuario no existe.
    ///     Retorna un <see cref="IActionResult"/> con código 500 Internal Server Error si ocurre un error inesperado.
    /// </returns>
    [HttpGet]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
      try
      {
        var response = await _userService.GetAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message }); ;
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    /// <summary>
    ///     Obtiene un usuario por su correo electrónico.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con código 200 OK si el usuario se obtiene correctamente.
    ///     Retorna un <see cref="IActionResult"/> con código 404 Not Found si el usuario no existe.
    ///     Retorna un <see cref="IActionResult"/> con código 500 Internal Server Error si ocurre un error inesperado.
    /// </returns>

    [HttpGet]
    [Route("email/{email}")]
    [Authorize]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
    {
      try
      {
        var response = await _userService.GetByEmailAsync(email);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message }); ;
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    /// <summary>
    ///     Actualiza la información de un usuario existente.
    /// </summary>
    /// <param name="userDto">Los datos del usuario a actualizar.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con código 200 OK si el usuario se actualiza correctamente.
    ///     Retorna un <see cref="IActionResult"/> con código 400 Bad Request si la solicitud es inválida.
    ///     Retorna un <see cref="IActionResult"/> con código 404 Not Found si el usuario no existe.
    ///     Retorna un <see cref="IActionResult"/> con código 500 Internal Server Error si ocurre un error inesperado.
    /// </returns>

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
    {
      try
      {
        var response = await _userService.UpdateAsync(userDto);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message }); ;
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message }); ;
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    /// <summary>
    ///     Elimina un usuario.
    /// </summary>
    /// <param name="id">El identificador del usuario a eliminar.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con código 200 OK si el usuario se elimina correctamente.
    ///     Retorna un <see cref="IActionResult"/> con código 404 Not Found si el usuario no existe.
    ///     Retorna un <see cref="IActionResult"/> con código 400 Bad Request si la solicitud es inválida.
    ///     Retorna un <see cref="IActionResult"/> con código 500 Internal Server Error si ocurre un error inesperado.
    /// </returns>
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Policy = "SuperadminOnly")]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
      try
      {
        var response = await _userService.DeleteAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message }); ;
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message }); ;
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    [HttpGet]
    [Route("transactions/{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserTransactions([FromRoute] string id)
    {
      try
      {
        var response = await _userService.GetUserTransactionsAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message }); ;
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    [HttpPost]
    [Route("topuprequest/create")]
    [Authorize]
    public async Task<IActionResult> CreateTopUpRequest([FromBody] TopUpRequestCreateDto topUpRequestDto)
    {
      try
      {
        var response = await _userService.CreateTopUpRequestAsync(topUpRequestDto);
        return Created(string.Empty, response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    [HttpPut]
    [Route("topuprequest/aprobe")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AproveOrRejectTopUp([FromBody] TopUpRequestUpdateDto topUpRequestDto)
    {
      try
      {
        var response = await _userService.AproveOrRejectTopUpAsync(topUpRequestDto);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    [HttpGet]
    [Route("topuprequest")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllTopUpRequests()
    {
      try
      {
        var response = await _userService.GetTopUpRequestsAsync();
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }

    [HttpGet]
    [Route("topuprequest/{id}")]
    [Authorize]
    public async Task<IActionResult> GetTopUpRequestById([FromRoute] string id)
    {
      try
      {
        var response = await _userService.GetTopUpRequestsByUserIdAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el usuario." });
      }
    }
  }
}
