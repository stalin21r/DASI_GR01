namespace Backend.Data {
  public class ApiResponse<T> {
    public int Status {
      get; set;
    }
    public string Message { get; set; } = string.Empty;
    public T? Data {
      get; set;
    }

    /// <summary>
    /// Constructor for ApiResponse
    /// </summary>
    /// <param name="status">Status of the response</param>
    /// <param name="message">Message of the response</param>
    /// <param name="data">Data of the response. Default is null</param>
    public ApiResponse(int status, string message, T? data = default) {
      Status = status;
      Message = message;
      Data = data;
    }
  }
}
