using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;

namespace Backend
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;

    //// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProductController"/>.
    /// </summary>
    /// <param name="productService">Una instancia de <see cref="IProductService"/> para realizar operaciones de productos.</param>
    public ProductController(IProductService productService)
    {
      _productService = productService;
    }

    /// <summary>
    ///     Crea un nuevo producto.
    /// </summary>
    /// <param name="productDto">Los datos del producto a crear.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status201Created"/> si el producto se crea correctamente.
    ///     En caso de error, retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status400BadRequest"/>.
    /// </returns>
    [HttpPost]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
    {
      try
      {
        var response = await _productService.CreateProductAsync(productDto);
        return Created(string.Empty, response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception)
      {
        return BadRequest("Error del servidor al crear el producto.");
      }
    }

    /// <summary>
    ///     Obtiene todos los productos.
    /// </summary>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status200OK"/> si los productos se obtienen correctamente.
    ///     En caso de error, retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status404NotFound"/> si no se encuentran los productos.
    ///     Si ocurre un error inesperado, retorna un <see cref="IActionResult"/> con un código de estado <see cref="StatusCodes.Status400BadRequest"/>.
    /// </returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllProducts()
    {
      try
      {
        var response = await _productService.GetAllProductsAsync();
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return BadRequest("Error del servidor al obtener los productos.");
      }
    }

    /// <summary>
    ///     Obtiene productos por tipo.
    /// </summary>
    /// <param name="type">El tipo de producto.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con código de estado <see cref="StatusCodes.Status200OK"/> si los productos se obtienen exitosamente.
    ///     En caso de error, retorna un <see cref="IActionResult"/> con código <see cref="StatusCodes.Status404NotFound"/> si no se encuentran productos.
    ///     Si ocurre un error inesperado, retorna un <see cref="IActionResult"/> con código <see cref="StatusCodes.Status400BadRequest"/>.
    /// </returns>
    [HttpGet("tipo/{type}")]
    [Authorize]
    public async Task<IActionResult> GetProductsByType([FromRoute] ProductType type)
    {
      try
      {
        var response = await _productService.GetProductsByTypeAsync(type);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return BadRequest("Error del servidor al obtener los productos.");
      }
    }

    /// <summary>
    ///     Obtiene un producto por su ID.
    /// </summary>
    /// <param name="id">El identificador del producto.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con código 200 OK si el producto se obtiene correctamente.
    ///     Retorna un <see cref="IActionResult"/> con código 404 Not Found si el producto no existe.
    ///     Retorna un <see cref="IActionResult"/> con código 400 Bad Request si ocurre un error inesperado.
    /// </returns>
    [HttpGet("{id:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> GetProductById([FromRoute] int id)
    {
      try
      {
        var response = await _productService.GetProductByIdAsync(id);
        return Ok(response);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return BadRequest("Error del servidor al obtener el producto.");
      }
    }

    /// <summary>
    ///     Actualiza un producto.
    /// </summary>
    /// <param name="productDto">Los datos del producto a actualizar.</param>
    /// <returns>
    ///     Retorna un <see cref="IActionResult"/> con código 200 OK si el producto se actualiza correctamente.
    ///     Retorna un <see cref="IActionResult"/> con código 400 Bad Request si la solicitud es inválida.
    ///     Retorna un <see cref="IActionResult"/> con código 400 Bad Request si ocurre un error inesperado.
    /// </returns>
    [HttpPut]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
    {
      try
      {
        var response = await _productService.UpdateProductAsync(productDto);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception)
      {
        return BadRequest("Error del servidor al actualizar el producto.");
      }
    }

		[HttpPost("sell")]
		[Authorize(Policy = "AdminPlus")]
		public async Task<IActionResult> SellProductAsync([FromBody] SellProductDto SellProductDto)
		{
			try
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var response = await _productService.SellProductAsync(SellProductDto, userId);
				return Ok(response);
			}
			catch (BadHttpRequestException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception)
			{
				return BadRequest("Error del servidor al actualizar el producto.");
			}
		}

		/// <summary>
		///     Elimina un producto.
		/// </summary>
		/// <param name="id">El identificador del producto a eliminar.</param>
		/// <returns>
		///     Retorna un <see cref="IActionResult"/> con código 200 (OK) si el producto se eliminó correctamente.
		///     Retorna un <see cref="IActionResult"/> con código 400 (Bad Request) si la solicitud es inválida.
		///     Retorna un <see cref="IActionResult"/> con código 404 (Not Found) si el producto no existe.
		///     Retorna un <see cref="IActionResult"/> con código 500 (Internal Server Error) para errores inesperados.
		/// </returns>
		[HttpDelete("{id:int:min(1)}")]
    [Authorize(Policy = "AdminPlus")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
      try
      {
        var response = await _productService.DeleteProductAsync(id);
        return Ok(response);
      }
      catch (BadHttpRequestException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception)
      {
        return BadRequest("Error del servidor al eliminar el producto.");
      }
    }
  }
}
