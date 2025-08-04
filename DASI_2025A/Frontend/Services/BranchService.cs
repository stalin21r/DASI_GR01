using System.Net.Http.Json;
using Shared;
namespace Frontend;

public class BranchService : IBranchService
{
  private readonly HttpClient _http;
  public BranchService(HttpClient http)
  {
    _http = http;
  }

  public async Task<ApiResponse<BranchDto>> CreateBranchAsync(BranchDto Branch)
  {
    try
    {
      var response = await _http.PostAsJsonAsync("api/v1/Branch", Branch);
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<BranchDto>(errorResponse.message)
          : new ApiResponse<BranchDto>("Error al crear la rama.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<BranchDto>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<BranchDto>("Error al crear rama");
    }
  }


  public async Task<ApiResponse<IEnumerable<BranchDto>>> GetAllBranches()
  {
    try
    {
      var response = await _http.GetAsync("api/v1/Branch");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<IEnumerable<BranchDto>>(errorResponse.message)
          : new ApiResponse<IEnumerable<BranchDto>>("Error al obtener ramas.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<BranchDto>>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<IEnumerable<BranchDto>>("Error al obtener ramas.");
    }
  }

  public async Task<ApiResponse<BranchDto>> GetBranchByIdAsync(string id)
  {
    try
    {
      var response = await _http.GetAsync($"api/v1/Branch/{id}");
      if (!response.IsSuccessStatusCode)
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return errorResponse != null
          ? new ApiResponse<BranchDto>(errorResponse.message)
          : new ApiResponse<BranchDto>("Error al obtener la rama.");
      }
      var result = await response.Content.ReadFromJsonAsync<ApiResponse<BranchDto>>();
      return result!;
    }
    catch
    {
      return new ApiResponse<BranchDto>("Error al obtener la rama.");
    }
  }
}