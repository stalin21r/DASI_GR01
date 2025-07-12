using Shared;

namespace Backend;

public class BranchService : IBranchService
{
  private readonly IBranchRepository _BranchRepository;
  public BranchService(IBranchRepository repository)
  {
    _BranchRepository = repository;
  }

  public async Task<ApiResponse<BranchDto>> CreateAsync(BranchDto BranchDto)
  {
    var result = await _BranchRepository.CreateAsync(BranchDto);
    if (result == null)
    {
      throw new BadHttpRequestException("Error al crear la Rama.");
    }
    ApiResponse<BranchDto> response = new ApiResponse<BranchDto>(
      message: "Rama creada exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  public async Task<ApiResponse<IEnumerable<BranchDto>>> GetAllAsync()
  {
    var result = await _BranchRepository.GetAllAsync();
    if (result == null || !result.Any())
    {
      throw new KeyNotFoundException("No se encontraron Ramas.");
    }
    ApiResponse<IEnumerable<BranchDto>> response = new ApiResponse<IEnumerable<BranchDto>>(
      message: "Ramas obtenidas exitosamente",
      data: result,
      totalRecords: result.Count()
    );
    return response;
  }

  public async Task<ApiResponse<BranchDto>> GetByIdAsync(int id)
  {
    var result = await _BranchRepository.GetByIdAsync(id);
    if (result == null)
    {
      throw new KeyNotFoundException("No se encontró la Rama.");
    }
    ApiResponse<BranchDto> response = new ApiResponse<BranchDto>(
      message: "Rama obtenida exitosamente",
      data: result,
      totalRecords: 1
    );
    return response;
  }

  public Task<ApiResponse<BranchDto>> UpdateAsync(BranchDto BranchDto)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResponse<bool>> DeleteAsync(int id)
  {
    throw new NotImplementedException();
  }
}