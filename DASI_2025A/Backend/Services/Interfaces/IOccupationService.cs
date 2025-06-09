using Shared;
namespace Backend;

public interface IOccupationService
{
  Task<ApiResponse<OccupationDto>> CreateAsync(OccupationDto occupationDto);
  Task<ApiResponse<OccupationDto>> GetByIdAsync(int id);
  Task<ApiResponse<IEnumerable<OccupationDto>>> GetAllAsync();
  Task<ApiResponse<OccupationDto>> UpdateAsync(OccupationDto occupationDto);
  Task<ApiResponse<bool>> DeleteAsync(int id);
}