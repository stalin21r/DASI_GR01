using Shared;

namespace Backend;

public class OccupationService : IOccupationService
{
  private readonly IOccupationRepository _occupationRepository;
  public OccupationService(IOccupationRepository repository)
  {
    _occupationRepository = repository;
  }

  public async Task<ApiResponse<OccupationDto>> CreateAsync(OccupationDto occupationDto)
  {
    var result = await _occupationRepository.CreateAsync(occupationDto);
    if (result == null)
    {
      throw new BadHttpRequestException("Error al crear la ocupación.");
    }
    ApiResponse<OccupationDto> response = new ApiResponse<OccupationDto>(
      message: "Ocupación creada exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  public Task<ApiResponse<bool>> DeleteAsync(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<ApiResponse<IEnumerable<OccupationDto>>> GetAllAsync()
  {
    var result = await _occupationRepository.GetAllAsync();
    if (result == null || !result.Any())
    {
      throw new KeyNotFoundException("No se encontraron ocupaciones.");
    }
    ApiResponse<IEnumerable<OccupationDto>> response = new ApiResponse<IEnumerable<OccupationDto>>(
      message: "Ocupaciones obtenidas exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }

  public async Task<ApiResponse<OccupationDto>> GetByIdAsync(int id)
  {
    var result = await _occupationRepository.GetByIdAsync(id);
    if (result == null)
    {
      throw new KeyNotFoundException("No se encontró la ocupación.");
    }
    ApiResponse<OccupationDto> response = new ApiResponse<OccupationDto>(
      message: "Ocupación obtenida exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  public Task<ApiResponse<OccupationDto>> UpdateAsync(OccupationDto occupationDto)
  {
    throw new NotImplementedException();
  }
}