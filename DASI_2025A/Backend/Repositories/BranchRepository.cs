using Microsoft.EntityFrameworkCore;
using Shared;
namespace Backend;

public class BranchRepository : IBranchRepository
{
  private readonly ApplicationDbContext _context;
  public BranchRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<BranchDto> CreateAsync(BranchDto BranchDto)
  {
    var entity = new BranchEntity
    {
      Name = BranchDto.Name
    };
    _context.Branches.Add(entity);
    await _context.SaveChangesAsync();
    return BranchDto;
  }


  public async Task<IEnumerable<BranchDto>> GetAllAsync()
  {
    var result = await _context.Branches.Select(o => new BranchDto
    {
      Id = o.Id,
      Name = o.Name
    }).ToListAsync();
    if (result == null || result.Count == 0)
    {
      throw new KeyNotFoundException("No se encontraron Ramas.");
    }
    return result;
  }

  public async Task<BranchDto> GetByIdAsync(int id)
  {
    var result = await _context.Branches.
    Where(o => o.Id == id).
    Select(
      o => new BranchDto
      {
        Id = o.Id,
        Name = o.Name
      })
    .FirstOrDefaultAsync();
    if (result == null)
    {
      throw new KeyNotFoundException("No se encontró la Rama.");
    }
    return result;
  }

  public Task<BranchDto> UpdateAsync(BranchDto BranchDto)
  {
    throw new NotImplementedException();
  }
  public Task<bool> DeleteAsync(int id)
  {
    throw new NotImplementedException();
  }

}