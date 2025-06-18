using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using System.Security.Claims;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProductController"/>.
    /// </summary>
    /// <param name="productService">Una instancia de <see cref="IProductService"/> para realizar operaciones de productos.</param>
    /// <param name="logger">Instancia de <see cref="ILogger"/> para registrar información.</param>
    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
      _productService = productService;
      _logger = logger;
    }

    /// <summary>
    ///     Crea un nuevo producto.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Usuario {UserId} intenta crear un nuevo producto: {@ProductDto}", userId, productDto);
        var response = await _productService.CreateProductAsync(productDto, userId!);
        _logger.LogInformation("Producto creado exitosamente por el usuario {UserId}.", userId);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud al crear producto: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al crear el producto.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al crear el producto." });
      }
    }

    /// <summary>
    ///     Obtiene todos los productos.
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllProducts()
    {
      try
      {
        _logger.LogInformation("Solicitud para obtener todos los productos.");
        var response = await _productService.GetAllProductsAsync();
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron productos: {Message}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al obtener los productos.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener los productos." });
      }
    }

    /// <summary>
    ///     Obtiene productos por tipo.
    /// </summary>
    [HttpGet("tipo/{type}")]
    [Authorize]
    public async Task<IActionResult> GetProductsByType([FromRoute] ProductType type)
    {
      try
      {
        _logger.LogInformation("Solicitud para obtener productos del tipo: {Type}", type);
        var response = await _productService.GetProductsByTypeAsync(type);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontraron productos del tipo {Type}: {Message}", type, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al obtener productos por tipo.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener los productos." });
      }
    }

    /// <summary>
    ///     Obtiene un producto por su ID.
    /// </summary>
    [HttpGet("{id:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> GetProductById([FromRoute] int id)
    {
      try
      {
        _logger.LogInformation("Solicitud para obtener producto con ID: {Id}", id);
        var response = await _productService.GetProductByIdAsync(id);
        _logger.LogInformation("Producto con ID {Id} obtenido exitosamente.", id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontró el producto con ID {Id}: {Message}", id, ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al obtener el producto por ID.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al obtener el producto." });
      }
    }

    /// <summary>
    ///     Actualiza un producto.
    /// </summary>
    [HttpPut]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto productDto)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Usuario {UserId} intenta actualizar el producto: {@ProductDto}", userId, productDto);
        var response = await _productService.UpdateProductAsync(productDto, userId!);
        _logger.LogInformation("Producto actualizado exitosamente por el usuario {UserId}.", userId);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud al actualizar producto: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogWarning("No se encontró el producto para actualizar: {Message}", ex.Message);
        return NotFound(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al actualizar el producto.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al actualizar el producto." });
      }
    }

    /// <summary>
    ///     Elimina un producto.
    /// </summary>
    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Usuario {UserId} intenta eliminar el producto con ID: {Id}", userId, id);
        var response = await _productService.DeleteProductAsync(id, userId!);
        _logger.LogInformation("Producto con ID {Id} eliminado exitosamente por el usuario {UserId}.", id, userId);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        _logger.LogWarning("Error de solicitud al eliminar producto: {Message}", ex.Message);
        return BadRequest(new { message = ex.Message });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error inesperado al eliminar el producto.");
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error del servidor al eliminar el producto." });
      }
    }
  }
}
