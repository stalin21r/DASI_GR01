using System.Net.Http.Headers;
using Blazored.LocalStorage;
public class AuthorizationMessageHandler : DelegatingHandler
{
  private readonly ILocalStorageService _localStorage;

  public AuthorizationMessageHandler(ILocalStorageService localStorage)
  {
    _localStorage = localStorage;
  }

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    var token = await _localStorage.GetItemAsync<string>("token");
    if (!string.IsNullOrWhiteSpace(token))
    {
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    else
    {
      Console.WriteLine("No token found");
    }
    return await base.SendAsync(request, cancellationToken);
  }
}
