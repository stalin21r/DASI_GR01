namespace Shared;

public class AdminTopUpRequestQueryParams
{
  public string? Type { get; set; }
  public string? Status { get; set; }
  public string? TargetUser { get; set; }
  public string? AuthorizedByUser { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }

  //Pagination
  private const int MaxPageSize = 50;
  public int PageNumber { get; set; } = 1;
  private int _pageSize = 10;
  public int PageSize
  {
    get => _pageSize;
    set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
  }
  public int Skip => (PageNumber - 1) * PageSize;
}