namespace Backend.Data
{
  public class ApiResponse<T>
  {
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public ApiResponse(int status, string message, T? data = default)
    {
      Status = status;
      Message = message;
      Data = data;
    }
  }
}
