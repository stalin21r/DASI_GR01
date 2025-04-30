using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;
public class ProductRepository : IProductRepository
{
  private readonly ApplicationDbContext _context;

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
    return productDto;
  }

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
