using Shared;
namespace Backend;

public interface IOccupationRepository
{
  Task<OccupationDto> CreateAsync(OccupationDto occupationDto);
  Task<OccupationDto> GetByIdAsync(int id);
  Task<IEnumerable<OccupationDto>> GetAllAsync();
  Task<OccupationDto> UpdateAsync(OccupationDto occupationDto);
  Task<bool> DeleteAsync(int id);

}