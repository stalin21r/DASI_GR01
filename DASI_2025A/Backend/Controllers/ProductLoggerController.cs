using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers
{
    public class ProductLoggerController : ControllerBase
    {
        private readonly IProductLoggerService _productLoggerService;

        public ProductLoggerController(IProductLoggerService productLoggerService)
        {
            _productLoggerService = productLoggerService;
        }

        public async Task<IActionResult> CreateProductLog([FromBody] ProductLoggerDto productLoggerDto)
        {
            try
            {
                var response = await _productLoggerService.CreateProductLogAsync(productLoggerDto);
                return Created(string.Empty, response);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Error del servidor al crear el log del producto.");
            }

        }
    }
}
