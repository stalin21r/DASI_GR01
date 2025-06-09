using System.Net.Http.Json;
using Shared;
namespace Frontend;

public class OccupationService : IOccupationService
{
  private readonly HttpClient _http;
  public OccupationService(HttpClient http)
  {
    _http = http;
  }

  public async Task<ApiResponse<OccupationDto>> CreateOccupationAsync(OccupationDto occupation)
  {
    try
    {
      var response = await _http.PostAsJsonAsync("api/v1/Occupation", occupation);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<OccupationDto>(errorResponse.message)
          : new ApiResponse<OccupationDto>("Error al crear la ocupación.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<OccupationDto>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<OccupationDto>("Error al crear ocupación");
    }
  }


  public async Task<ApiResponse<IEnumerable<OccupationDto>>> GetaAllOccupations()
  {
    try
    {
      var response = await _http.GetAsync("api/v1/Occupation");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<IEnumerable<OccupationDto>>(errorResponse.message)
          : new ApiResponse<IEnumerable<OccupationDto>>("Error al obtener Ocupaciones.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OccupationDto>>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<IEnumerable<OccupationDto>>("Error al obtener Ocupaciones.");
    }
  }

  public async Task<ApiResponse<OccupationDto>> GetOccupationByIdAsync(string id)
  {
    try
    {
      var response = await _http.GetAsync($"api/v1/Occupation/{id}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<OccupationDto>(errorResponse.message)
          : new ApiResponse<OccupationDto>("Error al obtener la ocupación.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<OccupationDto>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<OccupationDto>("Error al obtener la ocupación.");
    }
  }
}