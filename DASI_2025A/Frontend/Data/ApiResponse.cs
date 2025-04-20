namespace Frontend
{
  public class ApiResponse<T>
  {
    public int? status { get; set; }
    public string? message { get; set; }
    public T? data { get; set; }
  }
}
