using Microsoft.EntityFrameworkCore;
using Shared;
namespace Backend;

public class OccupationRepository : IOccupationRepository
{
  private readonly ApplicationDbContext _context;
  public OccupationRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<OccupationDto> CreateAsync(OccupationDto occupationDto)
  {
    var entity = new OccupationEntity
    {
      Name = occupationDto.Name
    };
    _context.Occupations.Add(entity);
    await _context.SaveChangesAsync();
    return occupationDto;
  }

  public Task<bool> DeleteAsync(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<OccupationDto>> GetAllAsync()
  {
    var result = await _context.Occupations.Select(o => new OccupationDto
    {
      Id = o.Id,
      Name = o.Name
    }).ToListAsync();
    if (result == null || result.Count == 0)
    {
      throw new KeyNotFoundException("No se encontraron ocupaciones.");
    }
    return result;
  }

  public async Task<OccupationDto> GetByIdAsync(int id)
  {
    var result = await _context.Occupations.
    Where(o => o.Id == id).
    Select(
      o => new OccupationDto
      {
        Id = o.Id,
        Name = o.Name
      })
    .FirstOrDefaultAsync();
    if (result == null)
    {
      throw new KeyNotFoundException("No se encontró la ocupación.");
    }
    return result;
  }

  public Task<OccupationDto> UpdateAsync(OccupationDto occupationDto)
  {
    throw new NotImplementedException();
  }
}