using Shared;
namespace Frontend;

public interface IBranchService
{
  Task<ApiResponse<BranchDto>> CreateBranchAsync(BranchDto Branch);
  Task<ApiResponse<IEnumerable<BranchDto>>> GetAllBranches();
  Task<ApiResponse<BranchDto>> GetBranchByIdAsync(string id);
}