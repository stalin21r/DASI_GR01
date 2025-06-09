using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class OccupationController : ControllerBase
  {
    private readonly IOccupationService _occupationService;

    public OccupationController(IOccupationService occupationService)
    {
      _occupationService = occupationService;
    }

    [HttpPost]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> CreateOccupation([FromBody] OccupationDto occupationDto)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _occupationService.CreateAsync(occupationDto);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la ocupación." });
      }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllOccupations()
    {
      try
      {
        var response = await _occupationService.GetAllAsync();
        return Ok(response);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener las ocupaciones." });
      }
    }

    [HttpGet("{id:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> GetOccupationById([FromRoute] int id)
    {
      try
      {
        var response = await _occupationService.GetByIdAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener la ocupación." });
      }
    }
  }
}