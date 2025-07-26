using Microsoft.EntityFrameworkCore;
using Shared;
namespace Backend;

public class BranchRepository : IBranchRepository
{
  private readonly ApplicationDbContext _context;
  /// <summary>
  ///   Inicializa una nueva instancia de <see cref="BranchRepository"/>.
  /// </summary>
  /// <param name="context">El contexto de la base de datos.</param>
  public BranchRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  /// <summary>
  ///   Crea una nueva Rama en la base de datos.
  /// </summary>
  /// <param name="BranchDto">El objeto con los datos de la Rama a crear.</param>
  /// <returns>El objeto con los datos de la Rama recién creada.</returns>
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


  /// <summary>
  ///   Obtiene todas las Ramas de la base de datos.
  /// </summary>
  /// <returns>
  ///     Retorna un <see cref="IEnumerable{BranchDto}"/> con los datos de todas las Ramas.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontraron Ramas.
  /// </returns>
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

  /// <summary>
  ///   Obtiene una Rama por su ID.
  /// </summary>
  /// <param name="id">El identificador de la Rama.</param>
  /// <returns>
  ///     Retorna un <see cref="BranchDto"/> con los datos de la Rama solicitada.
  ///     Lanza una excepción <see cref="KeyNotFoundException"/> si no se encontró la Rama.
  /// </returns>
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