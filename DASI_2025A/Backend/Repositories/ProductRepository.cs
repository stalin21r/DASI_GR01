using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;
public class ProductRepository : IProductRepository
{
  private readonly ApplicationDbContext _context;

  /// <summary>
  ///   Inicializa una nueva instancia de <see cref="ProductRepository"/>.
  /// </summary>
  /// <param name="context">El contexto de la base de datos.</param>
  public ProductRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<ProductDto> CreateAsync(ProductDto productDto)
  {
    var entity = new ProductEntity
    {
      Name = productDto.Name,
      Description = productDto.Description,
      Price = productDto.Price,
      Image = productDto.Image,
      Active = productDto.Active,
      Type = productDto.Type,
    };
    _context.Products.Add(entity);
    await _context.SaveChangesAsync();
    productDto.Id = entity.Id;
    return productDto;
  }

  /// <summary>
  ///   Obtiene todos los productos de la base de datos.
  /// </summary>
  /// <returns>
  ///   Retorna un <see cref="IList{ProductDto}"/> con todos los productos de la base de datos.
  /// </returns>
  public async Task<IEnumerable<ProductDto>> GetAllAsync()
  {
    return await _context.Products.Select(
      p => new ProductDto
      {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description ?? string.Empty,
        Price = p.Price,
        Image = p.Image,
        Active = p.Active,
        Type = p.Type
      }).ToListAsync();
  }

  /// <summary>
  ///   Obtiene una lista de productos filtrados por tipo.
  /// </summary>
  /// <param name="type">El tipo de producto a filtrar.</param>
  /// <returns>
  ///   Retorna una lista de <see cref="ProductDto"/> que representa los productos del tipo especificado.
  /// </returns>

  public async Task<IEnumerable<ProductDto>> GetByTypeAsync(ProductType type)
  {
    return await _context.Products.Where(p => p.Type == type).Select(
      p => new ProductDto
      {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description ?? string.Empty,
        Price = p.Price,
        Image = p.Image,
        Active = p.Active,
        Type = p.Type
      }).ToListAsync();
  }

  /// <summary>
  ///   Obtiene un producto por su ID.
  /// </summary>
  /// <param name="id">El identificador del producto a buscar.</param>
  /// <returns>
  ///   Retorna un <see cref="ProductDto"/> con los datos del producto si se encuentra.
  ///   Retorna <see langword="null"/> si no se encontró el producto.
  /// </returns>

  public async Task<ProductDto?> GetAsync(int id)
  {
    return await _context.Products.Where(p => p.Id == id).Select(
      p => new ProductDto
      {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description ?? string.Empty,
        Price = p.Price,
        Image = p.Image,
        Active = p.Active,
        Type = p.Type
      }
    ).FirstOrDefaultAsync();
  }

  /// <summary>
  ///   Actualiza un producto existente en la base de datos.
  /// </summary>
  /// <param name="productDto">Objeto con los datos del producto a actualizar.</param>
  /// <returns>
  ///   Retorna el objeto <see cref="ProductDto"/> actualizado si se encontró el producto.
  ///   Retorna <see langword="null"/> si no se encontró el producto.
  /// </returns>
  public async Task<ProductDto?> UpdateAsync(ProductDto productDto)
  {
    var entity = await _context.Products.FindAsync(productDto.Id);
    if (entity == null)
    {
      return null;
    }
    entity.Name = productDto.Name;
    entity.Description = productDto.Description;
    entity.Price = productDto.Price;
    entity.Image = productDto.Image;
    entity.Active = productDto.Active;
    entity.Type = productDto.Type;
    await _context.SaveChangesAsync();
    //Si es que el update es correcto instanciar un LoggerDto
    //luego del update se hace la llamda al LoggerRepositorio al metodo insertar log
    //al metodo insertar log se le pasaria una instancia mapeada de UpdateDto a LoggerDto

    return productDto;
  }

  //TODO: modificar el actual update para no modificar precio.
  //TODO: Crear un update para disminuir el stock (patch)
  //TODO: crear un nuevo dto para actualizar (updateProductoDto) -> agregar descripcion (no null), enviar el id de usuario que hace el update.

  /// <summary>
  ///   Elimina un producto existente en la base de datos, solo lo desactiva.
  /// </summary>
  /// <param name="id">El identificador del producto a eliminar.</param>
  /// <returns>
  ///   Retorna <see langword="true"/> si se encontró el producto y se eliminó correctamente.
  ///   Retorna <see langword="false"/> si no se encontró el producto.
  /// </returns>
  public async Task<bool> DeleteAsync(int id)
  {
    var entity = await _context.Products.FindAsync(id);
    if (entity == null)
    {
      return false;
    }
    entity.Active = false;
    await _context.SaveChangesAsync();
    return true;
  }
}
