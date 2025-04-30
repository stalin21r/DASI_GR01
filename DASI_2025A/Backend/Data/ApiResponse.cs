namespace Backend
{
  public class ApiResponse<T>
  {
    public string message { get; set; } = string.Empty;
    public T? data { get; set; }
    public int totalRecords { get; set; }

    public ApiResponse(string message, T? data = default, int totalRecords = 0)
    {
      this.message = message;
      this.data = data;
      this.totalRecords = totalRecords;
    }
  }
}
