using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace Backend
{

	[Route("api/v1/[controller]")]
	[ApiController]
	public class ProductLoggerController : ControllerBase
	{
		private readonly IProductLoggerService _productLoggerService;
		private readonly ILogger<ProductLoggerController> _logger;

		public ProductLoggerController(IProductLoggerService productLoggerService, ILogger<ProductLoggerController> logger)
		{
			_productLoggerService = productLoggerService;
			_logger = logger;
		}

		/// <summary>
		/// Obtiene todos los registros de log de productos.
		/// </summary>
		[HttpGet]
		[Authorize(Policy = "AdminPlus")]
		public async Task<IActionResult> GetAllProductLogs()
		{
			_logger.LogInformation("Solicitud para obtener todos los logs de productos recibida.");
			try
			{
				var response = await _productLoggerService.GetAllProductLogsAsync();
				_logger.LogInformation("Se obtuvieron correctamente todos los logs de productos.");
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener todos los logs de productos.");
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener los logs de productos." });
			}
		}

		/// <summary>
		/// Obtiene un registro de log por su ID.
		/// </summary>
		/// <param name="id">El ID del log a obtener.</param>
		[HttpGet("{id}")]
		[Authorize(Policy = "AdminPlus")]
		public async Task<IActionResult> GetProductLogById(int id)
		{
			_logger.LogInformation("Solicitud para obtener el log de producto con ID {Id} recibida.", id);
			try
			{
				var response = await _productLoggerService.GetProductLogByIdAsync(id);
				_logger.LogInformation("Se obtuvo correctamente el log de producto con ID {Id}.", id);
				return Ok(response);
			}
			catch (BadHttpRequestException ex)
			{
				_logger.LogWarning("No se encontró el log de producto con ID {Id}: {Message}", id, ex.Message);
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener el log de producto con ID {Id}.", id);
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener el log de producto." });
			}
		}

		/// <summary>
		/// Obtiene todos los logs asociados a un producto específico.
		/// </summary>
		/// <param name="productId">El ID del producto.</param>
		[HttpGet("product/{productId}")]
		[Authorize(Policy = "AdminPlus")]
		public async Task<IActionResult> GetProductLogsByProductId(int productId)
		{
			_logger.LogInformation("Solicitud para obtener logs del producto con ID {ProductId} recibida.", productId);
			try
			{
				var response = await _productLoggerService.GetProductLogsByProductIdAsync(productId);
				_logger.LogInformation("Se obtuvieron correctamente los logs del producto con ID {ProductId}.", productId);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener los logs del producto con ID {ProductId}.", productId);
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error del servidor al obtener logs del producto con ID {productId}." });
			}
		}
	}
}
