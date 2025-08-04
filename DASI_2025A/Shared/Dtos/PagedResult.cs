namespace Shared;

public class PagedResult<T>
{
  public IEnumerable<T> Items { get; set; } = new List<T>();
  public int TotalItems { get; set; }
}
