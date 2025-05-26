using Microsoft.AspNetCore.Identity;
using Shared;

namespace Backend;

public class ProductService : IProductService
{
  private readonly IProductRepository _productRepository;
  private readonly UserManager<ApplicationUser> _userManager;


  /// <summary>
  ///   Inicializa una nueva instancia de <see cref="ProductService"/>.</summary>
  /// <param name="repository">Una instancia de <see cref="IProductRepository"/> para realizar operaciones de productos.</param>
  public ProductService(
    IProductRepository productRepository,
    UserManager<ApplicationUser> userManager)
  {
    _productRepository = productRepository;
    _userManager = userManager;
  }

  /// <summary>
  ///     Crea un nuevo producto en el sistema.
  /// </summary>
  /// <param name="productDto">Los datos del producto a crear.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{ProductDto}"/> con los datos del producto recién creado.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo crear el producto.
  /// </returns>

  public async Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto, string userId)
  {
    var result = await _productRepository.CreateAsync(productDto, userId);
    if (result == null)
    {
      throw new BadHttpRequestException("Error al crear el producto.");
    }
    ApiResponse<ProductDto> response = new ApiResponse<ProductDto>(
      message: "Producto creado exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  /// <summary>
  ///     Obtiene todos los productos del sistema.
  /// </summary>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{IEnumerable{ProductDto}}"/> con todos los productos.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron productos.
  /// </returns>
  public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
  {
    var result = await _productRepository.GetAllAsync();
    if (result == null || !result.Any())
    {
      throw new KeyNotFoundException("No se encontraron Productos.");
    }
    ApiResponse<IEnumerable<ProductDto>> response = new ApiResponse<IEnumerable<ProductDto>>(
      message: "Productos obtenidos exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }

  /// <summary>
  ///     Obtiene productos por tipo del sistema.
  /// </summary>
  /// <param name="type">El tipo de producto a obtener.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{IEnumerable{ProductDto}}"/> con los productos del tipo especificado.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron productos del tipo especificado.
  /// </returns>

  public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByTypeAsync(ProductType type)
  {
    var result = await _productRepository.GetByTypeAsync(type);
    if (result == null || !result.Any())
    {
      throw new KeyNotFoundException("No se encontraron Productos.");
    }
    ApiResponse<IEnumerable<ProductDto>> response = new ApiResponse<IEnumerable<ProductDto>>(
      message: $"Productos {type} obtenidos exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }

  /// <summary>
  ///     Obtiene un producto por su ID.
  /// </summary>
  /// <param name="id">El identificador del producto.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{ProductDto}"/> con los datos del producto solicitado.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontró el producto.
  /// </returns>
  public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
  {
    var result = await _productRepository.GetAsync(id);
    if (result == null)
    {
      throw new KeyNotFoundException("No se encontraron Productos.");
    }
    ApiResponse<ProductDto> response = new ApiResponse<ProductDto>(
      message: $"Producto {result.Type} obtenido exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  /// <summary>
  ///     Actualiza un producto existente.
  /// </summary>
  /// <param name="productDto">Objeto con los datos del producto a actualizar.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{ProductDto}"/> con los datos del producto actualizado.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo actualizar el producto.
  /// </returns>
  public async Task<ApiResponse<ProductDto>> UpdateProductAsync(UpdateProductDto productDto, string userId)
  {
    var result = await _productRepository.UpdateAsync(productDto, userId);
    if (result == null)
    {
      throw new BadHttpRequestException("Error al actualizar el producto.");
    }
    ApiResponse<ProductDto> response = new ApiResponse<ProductDto>(
      message: "Producto actualizado exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  /// <summary>
  ///     Elimina un producto por su ID.
  /// </summary>
  /// <param name="id">El identificador del producto a eliminar.</param>
  /// <returns>
  ///     Retorna un <see cref="ApiResponse{bool}"/> con el resultado de la operación.
  ///     Lanza una excepción <see cref="BadHttpRequestException"/> si no se pudo eliminar el producto.
  /// </returns>
  public async Task<ApiResponse<bool>> DeleteProductAsync(int id, string userId)
  {
    var result = await _productRepository.DeleteAsync(id, userId);
    if (!result)
    {
      throw new BadHttpRequestException("Error al eliminar el producto.");
    }
    ApiResponse<bool> response = new ApiResponse<bool>(
      message: "Producto eliminado exitosamente",
      data: result,
      totalRecords: 0
    );
    return response;
  }
  /// <summary>
  /// Vende un producto, creando automáticamente una orden y actualizando el saldo del usuario
  /// </summary>

}
