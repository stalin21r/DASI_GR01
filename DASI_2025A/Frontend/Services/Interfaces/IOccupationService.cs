using Shared;
namespace Frontend;

public interface IOccupationService
{
  Task<ApiResponse<OccupationDto>> CreateOccupationAsync(OccupationDto occupation);
  Task<ApiResponse<IEnumerable<OccupationDto>>> GetaAllOccupations();
  Task<ApiResponse<OccupationDto>> GetOccupationByIdAsync(string id);
}