using Microsoft.EntityFrameworkCore;
using Shared;
namespace Backend;

public class OccupationRepository : IOccupationRepository
{
  private readonly ApplicationDbContext _context;

  /// <summary>
  /// Constructor, recibe el contexto de la base de datos.
  /// </summary>
  /// <param name="context">El contexto de la base de datos.</param>
  public OccupationRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Crea una nueva ocupación en la base de datos.
  /// </summary>
  /// <param name="occupationDto">Objeto con los datos de la ocupación a crear.</param>
  /// <returns>Objeto con los datos de la ocupación recién creada.</returns>
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

  /// <summary>
  /// Obtiene todas las ocupaciones de la base de datos.
  /// </summary>
  /// <returns>
  ///     Retorna un <see cref="IList{OccupationDto}"/> con todas las ocupaciones de la base de datos.
  ///     Si no se encontraron ocupaciones, lanza una excepción <see cref="KeyNotFoundException"/>.
  ///     Si ocurre un error inesperado, lanza una excepción <see cref="BadHttpRequestException"/>.
  /// </returns>
  public async Task<IEnumerable<OccupationDto>> GetAllAsync()
  {
    var result = await _context.Occupations.Select(o => new OccupationDto
    {
      Id = o.Id,
      Name = o.Name
    })
    .OrderByDescending(o => o.Id)
    .ToListAsync();
    if (result == null || result.Count == 0)
    {
      throw new KeyNotFoundException("No se encontraron ocupaciones.");
    }
    return result;
  }

  /// <summary>
  /// Obtiene una ocupación por su ID.
  /// </summary>
  /// <param name="id">El identificador de la ocupación.</param>
  /// <returns>
  ///     Retorna un <see cref="OccupationDto"/> con la información de la ocupación solicitada.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encentra  la ocupación.
  /// </returns>
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