namespace Shared;

public class UserQueryParams
{
  public string? SearchName { get; set; } // Nombre o Apellido
  public string? Role { get; set; }
  public int? OccupationFk { get; set; }
  public int? BranchFk { get; set; }
  public string? SearchEmail { get; set; }

  // Paginación
  private const int MaxPageSize = 50;
  private int _pageSize = 10;

  public int PageNumber { get; set; } = 1;

  public int PageSize
  {
    get => _pageSize;
    set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
  }

  public int Skip => (PageNumber - 1) * PageSize;
}
