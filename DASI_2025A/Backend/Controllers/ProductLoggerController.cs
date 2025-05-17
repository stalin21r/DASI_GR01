using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;


namespace Backend
{

	[Route("api/v1/[controller]")]
	[ApiController]
	public class ProductLoggerController : ControllerBase
	{
		private readonly IProductLoggerService _productLoggerService;

        //public async Task<IActionResult> CreateProductLog([FromBody] ProductLoggerDto productLoggerDto)
        //{
        //    try
        //    {
        //        var response = await _productLoggerService.CreateProductLogAsync(productLoggerDto);
        //        return Created(string.Empty, response);
        //    }
        //    catch (BadHttpRequestException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Error del servidor al crear el log del producto.");
        //    }

        //}
        public ProductLoggerController(IProductLoggerService productLoggerService)
		{
			_productLoggerService = productLoggerService;
		}

		/// <summary>
		/// Obtiene todos los registros de log de productos.
		/// </summary>
		[HttpGet]
		[Authorize(Policy = "AdminPlus")]
		public async Task<IActionResult> GetAllProductLogs()
		{
			try
			{
				var response = await _productLoggerService.GetAllProductLogsAsync();
				return Ok(response);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
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
			try
			{
				var response = await _productLoggerService.GetProductLogByIdAsync(id);
				return Ok(response);
			}
			catch (BadHttpRequestException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (Exception)
			{
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
			try
			{
				var response = await _productLoggerService.GetProductLogsByProductIdAsync(productId);
				return Ok(response);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error del servidor al obtener logs del producto con ID {productId}." });
			}
		}
	}
}
