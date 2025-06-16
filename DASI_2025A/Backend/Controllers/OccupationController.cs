using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using System.Security.Claims;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class OccupationController : ControllerBase
  {
    private readonly IOccupationService _occupationService;
    private readonly ILogger<OccupationController> _logger;

    public OccupationController(IOccupationService occupationService, ILogger<OccupationController> logger)
    {
      _occupationService = occupationService;
      _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> CreateOccupation([FromBody] OccupationDto occupationDto)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Usuario {UserId} intenta crear una nueva ocupación.", userId);
        var response = await _occupationService.CreateAsync(occupationDto);
        _logger.LogInformation("Ocupación creada exitosamente por el usuario {UserId}.", userId);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud al crear ocupación: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error del servidor al crear la ocupación.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la ocupación." });
      }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllOccupations()
    {
      try
      {
        _logger.LogInformation("Solicitud para obtener todas las ocupaciones.");
        var response = await _occupationService.GetAllAsync();
        _logger.LogInformation("Ocupaciones obtenidas exitosamente.");
        return Ok(response);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error del servidor al obtener las ocupaciones.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener las ocupaciones." });
      }
    }

    [HttpGet("{id:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> GetOccupationById([FromRoute] int id)
    {
      try
      {
        _logger.LogInformation("Solicitud para obtener la ocupación con ID {Id}.", id);
        var response = await _occupationService.GetByIdAsync(id);
        _logger.LogInformation("Ocupación con ID {Id} obtenida exitosamente.", id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontró la ocupación con ID {Id}: {Message}", id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error del servidor al obtener la ocupación con ID {Id}.", id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener la ocupación." });
      }
    }
  }
}