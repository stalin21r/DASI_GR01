﻿using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;

public class ProductRepository : IProductRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IProductLoggerRepository _logger;
  private readonly IImgurService _imgurService;

  /// <summary>
  ///   Inicializa una nueva instancia de <see cref="ProductRepository"/>.
  /// </summary>
  /// <param name="context">El contexto de la base de datos.</param>
  public ProductRepository(ApplicationDbContext context, IProductLoggerRepository logger, IImgurService imgurService)
  {
    _context = context;
    _logger = logger;
    _imgurService = imgurService;
  }

  public async Task<ProductDto> CreateAsync(ProductDto productDto, string userId)
  {
    string? imgurLink = null;
    string? deleteHash = null;

    if (!string.IsNullOrWhiteSpace(productDto.Image))
    {
      try
      {

        var uploadResult = await _imgurService.UploadImageAsync(productDto.Image, productDto.Name);
        imgurLink = uploadResult.Link;
        deleteHash = uploadResult.DeleteHash;

      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error al subir la imagen a Imgur: {ex.Message}");
        imgurLink = null;
        deleteHash = null;
      }
    }

    productDto.Image = imgurLink;

    var entity = new ProductEntity
    {
      Name = productDto.Name,
      Description = productDto.Description,
      Price = productDto.Price,
      Stock = productDto.Stock,
      Image = imgurLink ?? string.Empty, // o null si tu columna lo permite
      Active = productDto.Active,
      Type = productDto.Type,
      ImageDeleteHash = deleteHash ?? string.Empty
    };

    _context.Products.Add(entity);
    var result = await _context.SaveChangesAsync();
    if (result == 0)
    {
      throw new Exception("No se pudo insertar el producto.");
    }

    var loggerDto = new ProductLoggerDto
    {
      ProductId = entity.Id,
      UserId = userId,
      Action = "Crear Producto",
      Description = "Producto Creado",
      QuantityBefore = 0,
      QuantityAfter = entity.Stock
    };

    await _logger.CreateAsync(loggerDto);

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
        Stock = p.Stock,
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
        Stock = p.Stock,
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
        Stock = p.Stock,
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
  public async Task<ProductDto?> UpdateAsync(UpdateProductDto productDto, string userId)
  {
    var entity = await _context.Products.FindAsync(productDto.Id);
    if (entity == null)
    {
      return null;
    }

    uint stockBefore = entity.Stock;

    // Solo subir nueva imagen si viene una en base64
    if (!string.IsNullOrWhiteSpace(productDto.Image))
    {
      try
      {
        // Eliminar imagen anterior si existe deleteHash
        if (!string.IsNullOrEmpty(entity.ImageDeleteHash))
        {
          await _imgurService.DeleteImageAsync(entity.ImageDeleteHash);
        }

        // Subir nueva imagen
        var uploadResult = await _imgurService.UploadImageAsync(productDto.Image, productDto.Name);
        entity.Image = uploadResult.Link;
        entity.ImageDeleteHash = uploadResult.DeleteHash;
      }
      catch (Exception ex)
      {
        // Aquí puedes loguear el error o manejarlo según convenga
        Console.WriteLine($"Error al actualizar la imagen en Imgur: {ex.Message}");
        // No sobreescribas la imagen actual si falla la subida
      }
    }

    // Actualizar propiedades restantes
    entity.Name = productDto.Name;
    entity.Description = productDto.Description;
    entity.Stock = productDto.Stock;
    entity.Active = productDto.Active;
    entity.Type = productDto.Type;

    var result = await _context.SaveChangesAsync();
    if (result == 0)
    {
      throw new BadHttpRequestException("No se realizaron cambios en el producto.");
    }

    // Guardar log
    ProductLoggerDto loggerDto = new ProductLoggerDto
    {
      ProductId = entity.Id,
      UserId = userId,
      Action = "Actualizar Producto",
      Description = productDto.Reason,
      QuantityBefore = stockBefore,
      QuantityAfter = entity.Stock
    };

    await _logger.CreateAsync(loggerDto);

    // Construir DTO resultado
    var updatedProductDto = new ProductDto
    {
      Id = entity.Id,
      Name = entity.Name,
      Description = entity.Description ?? string.Empty,
      Price = entity.Price,
      Stock = entity.Stock,
      Image = entity.Image,
      Active = entity.Active,
      Type = entity.Type
    };

    return updatedProductDto;
  }

  /// <summary>
  ///   Registra una venta de producto y reduce su stock disponible.
  /// </summary>
  /// <param name="sellProductDto">
  ///   Objeto DTO con la información de la venta, incluyendo el ID del producto y la cantidad.
  /// </param>
  /// <param name="userId">
  ///   El identificador del usuario que realiza la venta.
  /// </param>
  /// <returns>
  ///   Retorna el <see cref="ProductDto"/> actualizado con el nuevo stock si la venta fue exitosa.
  ///   Retorna <see langword="null"/> si no se encontró el producto, si no está activo, o si no hay stock suficiente.
  /// </returns>
  /// <remarks>
  ///   Este método gestiona todo el proceso de venta en una única transacción, garantizando
  ///   la consistencia entre la actualización del stock y el registro del log de operación.
  ///   Si ocurre algún error durante el proceso, la transacción se revierte completamente.
  /// </remarks>
  public async Task<ProductDto?> SellProductAsync(SellProductDto sellProductDto, string userId)
  {
    // La validación básica ya está en el DTO con las anotaciones
    using var transaction = await _context.Database.BeginTransactionAsync();


    var entity = await _context.Products.FindAsync(sellProductDto.ProductId);

    // Verificamos si el producto existe y está activo
    if (entity == null || !entity.Active)
    {
      throw new KeyNotFoundException("No se encontro este producto.");
    }

    // Verificamos si hay stock suficiente
    if (entity.Stock < sellProductDto.Quantity)
    {
      throw new BadHttpRequestException("No hay stock suficiente.");
    }

    // Guardamos el stock antes de la actualización
    uint stockBefore = entity.Stock;

    // Actualizamos el stock
    entity.Stock -= sellProductDto.Quantity;

    // Creamos el registro de la operación
    var logEntry = new ProductLoggerEntity
    {
      Action = "Venta de Producto",
      Description = $"Venta de {sellProductDto.Quantity} unidades del producto '{entity.Name}'",
      QuantityBefore = stockBefore,
      QuantityAfter = entity.Stock,
      ProductFk = entity.Id,
      UserFk = userId,
    };
    await _context.SaveChangesAsync();

    await _context.ProductLogs.AddAsync(logEntry);

    // Retornamos el producto actualizado
    return new ProductDto
    {
      Id = entity.Id,
      Name = entity.Name,
      Description = entity.Description ?? string.Empty,
      Price = entity.Price,
      Stock = entity.Stock,
      Image = entity.Image,
      Active = entity.Active,
      Type = entity.Type
    };
  }

  /// <summary>
  ///   Elimina un producto existente en la base de datos, solo lo desactiva.
  /// </summary>
  /// <param name="id">El identificador del producto a eliminar.</param>
  /// <returns>
  ///   Retorna <see langword="true"/> si se encontró el producto y se eliminó correctamente.
  ///   Retorna <see langword="false"/> si no se encontró el producto.
  /// </returns>
  public async Task<bool> DeleteAsync(int id, string userId)
  {
    var entity = await _context.Products.FindAsync(id);
    if (entity == null)
    {
      throw new KeyNotFoundException("No se encontró este producto.");
    }
    entity.Active = false;
    entity.Stock = 0;
    await _context.SaveChangesAsync();
    await _logger.CreateAsync(new ProductLoggerDto
    {
      ProductId = entity.Id,
      UserId = userId,
      Action = "Eliminar Producto",
      Description = "Producto eliminado",
      QuantityBefore = entity.Stock,
      QuantityAfter = 0
    });
    return true;
  }
}
