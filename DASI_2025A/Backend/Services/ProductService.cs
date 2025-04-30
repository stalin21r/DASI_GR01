using Shared;

namespace Backend;

public class ProductService : IProductService
{
  private readonly IProductRepository _repository;

  public ProductService(IProductRepository repository)
  {
    _repository = repository;
  }

  public async Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto productDto)
  {
    var result = await _repository.CreateAsync(productDto);
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


  public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
  {
    var result = await _repository.GetAllAsync();
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

  public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByTypeAsync(ProductType type)
  {
    var result = await _repository.GetByTypeAsync(type);
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

  public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
  {
    var result = await _repository.GetAsync(id);
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

  public async Task<ApiResponse<ProductDto>> UpdateProductAsync(ProductDto productDto)
  {
    var result = await _repository.UpdateAsync(productDto);
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

  public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
  {
    var result = await _repository.DeleteAsync(id);
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
}
