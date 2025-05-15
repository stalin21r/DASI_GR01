using System.Net.Http.Headers;
using Newtonsoft.Json;
using Shared;

namespace Backend;

public class ImgurService : IImgurService
{
  private readonly IConfiguration _configuration;
  private readonly IHttpClientFactory _httpClientFactory;

  public ImgurService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
  {
    _configuration = configuration;
    _httpClientFactory = httpClientFactory;
  }

  public async Task<ImgurUploadResultDto> UploadImageAsync(string imageBase64, string title)
  {
    // Obtener el token de la base de datos
    string token = GetImgurTokenFromDatabaseAsync();

    if (string.IsNullOrEmpty(token))
    {
      throw new Exception("No se ha encontrado el token de imgur");
    }

    var httpClient = _httpClientFactory.CreateClient();
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var formData = new MultipartFormDataContent();
    formData.Add(new StringContent(imageBase64), "image");
    formData.Add(new StringContent("base64"), "type");
    formData.Add(new StringContent(title), "title");

    try
    {
      var response = await httpClient.PostAsync("https://api.imgur.com/3/image", formData);
      var responseContent = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        var errorData = JsonConvert.DeserializeObject<ImgurErrorResponse>(responseContent);
        throw new Exception($"Imgur API error: {errorData?.Data?.Error ?? "Unknown error"}");
      }

      var result = JsonConvert.DeserializeObject<ImgurResponse>(responseContent);
      return new ImgurUploadResultDto
      {
        Link = result?.Data?.Link,
        DeleteHash = result?.Data?.DeleteHash
      };
    }
    catch (HttpRequestException httpEx)
    {
      throw new Exception("Error al comunicarse con el servidor de Imgur", httpEx);
    }
    catch (Exception ex)
    {
      throw new Exception("Error al subir la imagen a Imgur", ex);
    }
  }

  public async Task DeleteImageAsync(string deletehash)
  {
    var clientId = _configuration["Imgur:ImgurClientId"];
    var httpClient = _httpClientFactory.CreateClient();
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", clientId);

    try
    {
      var response = await httpClient.DeleteAsync($"https://api.imgur.com/3/image/{deletehash}");
      var responseContent = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        var errorData = JsonConvert.DeserializeObject<ImgurErrorResponse>(responseContent);
        throw new Exception($"Imgur API error: {errorData?.Data?.Error ?? "Unknown error"}");
      }
    }
    catch (HttpRequestException httpEx)
    {
      throw new Exception("Error al comunicarse con el servidor de Imgur", httpEx);
    }
    catch (Exception ex)
    {
      throw new Exception("Error al eliminar la imagen de Imgur", ex);
    }
  }

  private string GetImgurTokenFromDatabaseAsync()
  {

    string token = _configuration["Imgur:ImgutToken"]?.ToString() ?? string.Empty;
    return token;
  }
}
