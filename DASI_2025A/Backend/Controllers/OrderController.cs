using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class OrderController : ControllerBase
  {
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
      _orderService = orderService;
    }

    [HttpPost]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] OrderCreateDto order)
    {
      try
      {
        var response = await _orderService.CreateOrderAsync(order);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la orden." });
      }
    }

    [HttpGet]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
      try
      {
        var response = await _orderService.GetAllOrdersAsync();
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
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la orden." });
      }
    }

    [HttpGet("buyer-orders/{buyerId}")]
    [Authorize]
    public async Task<IActionResult> GetOrdersByBuyerIdAsync([FromRoute] string buyerId)
    {
      try
      {
        var response = await _orderService.GetOrdersByBuyerIdAsync(buyerId);
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
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la orden." });
      }
    }

    [HttpGet("seller-orders/{sellerId}")]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> GetOrdersBySellerIdAsync([FromRoute] string sellerId)
    {
      try
      {
        var response = await _orderService.GetOrdersBySellerIdAsync(sellerId);
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
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear la orden." });
      }
    }


  }
}