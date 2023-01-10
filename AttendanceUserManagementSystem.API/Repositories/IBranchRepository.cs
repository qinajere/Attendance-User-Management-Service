using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Resources.DTO;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public interface IBranchRepository
    {
        Task<bool> AddBranch(Branch branch);
        Task<bool> UpdateBranch(Branch branch);
        Task<Branch> GetBranchById(int id);
        Task<List<Branch>> GetAllBranches();


    }
}
