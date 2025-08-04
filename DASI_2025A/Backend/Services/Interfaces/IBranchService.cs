using Shared;
namespace Backend;

public interface IBranchService
{
  Task<ApiResponse<BranchDto>> CreateAsync(BranchDto BranchDto);
  Task<ApiResponse<BranchDto>> GetByIdAsync(int id);
  Task<ApiResponse<IEnumerable<BranchDto>>> GetAllAsync();
  Task<ApiResponse<BranchDto>> UpdateAsync(BranchDto BranchDto);
  Task<ApiResponse<bool>> DeleteAsync(int id);
}