using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;

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
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderDto orderDto)
        {
            try
            {
                var response = await _orderService.CreateOrderAsync(orderDto);
                return Created(string.Empty, response);
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
