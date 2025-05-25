using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentAsync([FromBody] PaymentDto paymentDto)
        {
            try
            {
                var response = await _paymentService.CreatePaymentAsync(paymentDto);
                return Created(string.Empty, response);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el producto." });
            }
        }
    }
}
