using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using System.Security.Claims;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class BranchController : ControllerBase
  {
    private readonly IBranchService _BranchService;
    private readonly ILogger<BranchController> _logger;

    public BranchController(IBranchService BranchService, ILogger<BranchController> logger)
    {
      _BranchService = BranchService;
      _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> CreateBranch([FromBody] BranchDto BranchDto)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Usuario {UserId} intenta crear una nueva Rama.", userId);
        var response = await _BranchService.CreateAsync(BranchDto);
        _logger.LogInformation("Rama creada exitosamente por el usuario {UserId}.", userId);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud al crear Rama: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error del servidor al crear la Rama.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la Rama." });
      }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllBranchs()
    {
      try
      {
        _logger.LogInformation("Solicitud para obtener todas las Ramas.");
        var response = await _BranchService.GetAllAsync();
        _logger.LogInformation("Ramas obtenidas exitosamente.");
        return Ok(response);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error del servidor al obtener las Ramas.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener las Ramas." });
      }
    }

    [HttpGet("{id:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> GetBranchById([FromRoute] int id)
    {
      try
      {
        _logger.LogInformation("Solicitud para obtener la Rama con ID {Id}.", id);
        var response = await _BranchService.GetByIdAsync(id);
        _logger.LogInformation("Rama con ID {Id} obtenida exitosamente.", id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontró la Rama con ID {Id}: {Message}", id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error del servidor al obtener la Rama con ID {Id}.", id);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener la Rama." });
      }
    }
  }
}