using System.Net.Http.Json;
using Shared;

namespace Frontend {
  public class CombosService {
    private readonly HttpClient _http;

    public CombosService(HttpClient http) {
      _http = http;
    }

    public async Task<ApiResponse<List<CombosDTO>>> GetCombosAsync() {
      try {
        var response = await _http.GetFromJsonAsync<ApiResponse<List<CombosDTO>>>("/api/Combos");
        Console.WriteLine(response);
        if (response == null || response.status != 200) {
          throw new HttpRequestException("Error al obtener los combos.");
        }
        return response;
      }
      catch (Exception ex) {
        return new ApiResponse<List<CombosDTO>>(
          500,
          $"Error en la solicitud: {ex.Message}",
          null
        );
      }
    }

    public async Task<ApiResponse<CombosDTO>> GetComboAsync(int id) {
      try {
        var response = await _http.GetFromJsonAsync<ApiResponse<CombosDTO>>($"/api/Combos/{id}");
        if (response == null || response.status != 200) {
          throw new HttpRequestException("Error al obtener el combo.");
        }
        return response;
      }
      catch (Exception ex) {
        return new ApiResponse<CombosDTO>(
          500,
          $"Error en la solicitud: {ex.Message}",
          null
        );
      }
    }

    public async Task<ApiResponse<CombosDTO>> PostComboAsync(CombosDTO combo) {
      try {
        var response = await _http.PostAsJsonAsync("/api/Combos", combo);
        if (response.IsSuccessStatusCode) {
          return new ApiResponse<CombosDTO>(
            200,
            "Combo actualizado con éxito.",
            await response.Content.ReadFromJsonAsync<CombosDTO>()
          );
        }
        else {
          return new ApiResponse<CombosDTO>(
            400,
            "No se pudo crear el combo.",
            null
          );
        }
      }
      catch (Exception ex) {
        return new ApiResponse<CombosDTO>(
          500,
          $"Error en la solicitud: {ex.Message}",
          null
        );
      }
    }

    public async Task<ApiResponse<bool>> PutComboAsync(int id, CombosDTO combo) {
      try {
        var response = await _http.PutAsJsonAsync($"/api/Combos/{id}", combo);
        if (response.IsSuccessStatusCode) {
          return new ApiResponse<bool>(
            200,
            "Combo actualizado con éxito.",
            true
          );
        }
        else {
          return new ApiResponse<bool>(
            400,
            "No se pudo actualizar el combo.",
            false
          );
        }
      }
      catch (Exception ex) {
        return new ApiResponse<bool>(
          500,
          $"Error en la solicitud: {ex.Message}",
          false
        );
      }
    }

    public async Task<ApiResponse<bool>> DeleteComboAsync(int id) {
      try {
        var response = await _http.DeleteAsync($"/api/Combos/{id}");
        if (response.IsSuccessStatusCode) {
          return new ApiResponse<bool>(
            200,
            "Combo eliminado con éxito.",
            true
          );
        }
        else {
          return new ApiResponse<bool>(
            400,
            "No se pudo eliminar el combo.",
            false
          );
        }
      }
      catch (Exception ex) {
        return new ApiResponse<bool>(
          500,
          $"Error en la solicitud: {ex.Message}",
          false
        );
      }
    }
  }
}