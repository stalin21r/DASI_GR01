using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class OrderController : ControllerBase
  {
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
      _orderService = orderService;
      _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] OrderCreateDto order)
    {
      _logger.LogInformation("Intentando crear una nueva orden.");
      try
      {
        var response = await _orderService.CreateOrderAsync(order);
        _logger.LogInformation("Orden creada exitosamente.");
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud incorrecta al crear la orden: {Mensaje}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontró un recurso necesario al crear la orden: {Mensaje}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (InvalidOperationException ex)
      {
        _logger.LogWarning("Operación inválida al crear la orden: {Mensaje}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado del servidor al crear la orden.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la orden." });
      }
    }

    [HttpGet]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> GetAllOrdersAsync([FromQuery] OrderQueryParams queryParams)
    {
      _logger.LogInformation("Obteniendo órdenes con filtros: {@QueryParams}", queryParams);

      try
      {
        var response = await _orderService.GetAllOrdersAsync(queryParams);
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
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado del servidor al obtener las órdenes.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor." });
      }
    }


    [HttpGet("buyer-orders/{buyerId}")]
    [Authorize]
    public async Task<IActionResult> GetOrdersByBuyerIdAsync([FromRoute] string buyerId)
    {
      _logger.LogInformation("Obteniendo órdenes para el comprador con ID: {BuyerId}", buyerId);
      try
      {
        var response = await _orderService.GetOrdersByBuyerIdAsync(buyerId);
        _logger.LogInformation("Órdenes del comprador {BuyerId} obtenidas exitosamente.", buyerId);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron órdenes para el comprador {BuyerId}: {Mensaje}", buyerId, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud incorrecta al obtener órdenes del comprador {BuyerId}: {Mensaje}", buyerId, ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado del servidor al obtener las órdenes del comprador {BuyerId}.", buyerId);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la orden." });
      }
    }

    [HttpGet("seller-orders/{sellerId}")]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> GetOrdersBySellerIdAsync([FromRoute] string sellerId)
    {
      _logger.LogInformation("Obteniendo órdenes para el vendedor con ID: {SellerId}", sellerId);
      try
      {
        var response = await _orderService.GetOrdersBySellerIdAsync(sellerId);
        _logger.LogInformation("Órdenes del vendedor {SellerId} obtenidas exitosamente.", sellerId);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron órdenes para el vendedor {SellerId}: {Mensaje}", sellerId, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Solicitud incorrecta al obtener órdenes del vendedor {SellerId}: {Mensaje}", sellerId, ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado del servidor al obtener las órdenes del vendedor {SellerId}.", sellerId);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la orden." });
      }
    }
  }
}