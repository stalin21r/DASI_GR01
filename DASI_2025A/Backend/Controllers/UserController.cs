using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
      try
      {
        var response = await _userService.CreateAsync(userDto);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Error del servidor al crear el usuario.");
      }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
      try
      {
        var response = await _userService.GetAllAsync();
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Error del servidor al obtener los usuarios.");
      }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
      try
      {
        var response = await _userService.GetAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Error del servidor al obtener el usuario.");
      }
    }

    [HttpGet]
    [Route("email/{email}")]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
    {
      try
      {
        var response = await _userService.GetByEmailAsync(email);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Error del servidor al obtener el usuario.");
      }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
    {
      try
      {
        var response = await _userService.UpdateAsync(userDto);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Error del servidor al actualizar el usuario.");
      }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
      try
      {
        var response = await _userService.DeleteAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Error del servidor al eliminar el usuario.");
      }
    }
  }
}
