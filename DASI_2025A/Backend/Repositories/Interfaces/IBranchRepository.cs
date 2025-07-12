using Shared;
namespace Backend;

public interface IBranchRepository
{
  Task<BranchDto> CreateAsync(BranchDto BranchDto);
  Task<BranchDto> GetByIdAsync(int id);
  Task<IEnumerable<BranchDto>> GetAllAsync();
  Task<BranchDto> UpdateAsync(BranchDto BranchDto);
  Task<bool> DeleteAsync(int id);

}