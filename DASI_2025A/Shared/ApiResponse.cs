namespace Shared;
public class ApiResponse<T> {
  public int status { get; set; }
  public string message { get; set; } = string.Empty;
  public T? data { get; set; }
  public ApiResponse(int status, string message, T? data = default) {
    this.status = status;
    this.message = message;
    this.data = data;
  }
}

