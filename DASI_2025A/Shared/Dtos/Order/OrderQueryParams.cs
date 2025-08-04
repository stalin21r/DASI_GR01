namespace Shared;

public class OrderQueryParams
{
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  public string? SellerFullName { get; set; }
  public string? BuyerFullName { get; set; }

  //Paginacion
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