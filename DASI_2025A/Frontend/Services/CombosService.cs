using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace Frontend
{
  public class CombosService
  {
    private readonly HttpClient _http;

    public CombosService(HttpClient http)
    {
      _http = http;
    }

    public async Task<ApiResponse<List<Combos>>> GetCombosAsync()
    {
      try
      {
        var response = await _http.GetFromJsonAsync<ApiResponse<List<Combos>>>("/api/Combos");
        Console.WriteLine(response);
        if (response == null || response.status != 200)
        {
          throw new HttpRequestException("Error al obtener los combos.");
        }
        return response;
      }
      catch (Exception ex)
      {
        return new ApiResponse<List<Combos>>
        {
          status = 500,
          message = $"Error en la solicitud: {ex.Message}",
          data = null
        };
      }
    }

    public async Task<ApiResponse<Combos>> GetComboAsync(int id)
    {
      try
      {
        var response = await _http.GetFromJsonAsync<ApiResponse<Combos>>($"/api/Combos/{id}");
        if (response == null || response.status != 200)
        {
          throw new HttpRequestException("Error al obtener el combo.");
        }
        return response;
      }
      catch (Exception ex)
      {
        return new ApiResponse<Combos>
        {
          status = 500,
          message = $"Error en la solicitud: {ex.Message}",
          data = null
        };
      }
    }

    public async Task<ApiResponse<Combos>> PostComboAsync(Combos combo)
    {
      try
      {
        var response = await _http.PostAsJsonAsync("/api/Combos", combo);
        if (response.IsSuccessStatusCode)
        {
          return new ApiResponse<Combos>
          {
            status = 200,
            message = "Combo actualizado con éxito.",
            data = await response.Content.ReadFromJsonAsync<Combos>()
          };
        }
        else
        {
          return new ApiResponse<Combos>
          {
            status = 400,
            message = "No se pudo crear el combo.",
            data = null
          };
        }
      }
      catch (Exception ex)
      {
        return new ApiResponse<Combos>
        {
          status = 500,
          message = $"Error en la solicitud: {ex.Message}",
          data = null
        };
      }
    }

    public async Task<ApiResponse<bool>> PutComboAsync(int id, Combos combo)
    {
      try
      {
        var response = await _http.PutAsJsonAsync($"/api/Combos/{id}", combo);
        if (response.IsSuccessStatusCode)
        {
          return new ApiResponse<bool>
          {
            status = 200,
            message = "Combo actualizado con éxito.",
            data = true
          };
        }
        else
        {
          return new ApiResponse<bool>
          {
            status = 400,
            message = "No se pudo actualizar el combo.",
            data = false
          };
        }
      }
      catch (Exception ex)
      {
        return new ApiResponse<bool>
        {
          status = 500,
          message = $"Error en la solicitud: {ex.Message}",
          data = false
        };
      }
    }

    public async Task<ApiResponse<bool>> DeleteComboAsync(int id)
    {
      try
      {
        var response = await _http.DeleteAsync($"/api/Combos/{id}");
        if (response.IsSuccessStatusCode)
        {
          return new ApiResponse<bool>
          {
            status = 200,
            message = "Combo eliminado con éxito.",
            data = true
          };
        }
        else
        {
          return new ApiResponse<bool>
          {
            status = 400,
            message = "No se pudo eliminar el combo.",
            data = false
          };
        }
      }
      catch (Exception ex)
      {
        return new ApiResponse<bool>
        {
          status = 500,
          message = $"Error en la solicitud: {ex.Message}",
          data = false
        };
      }
    }
  }
}