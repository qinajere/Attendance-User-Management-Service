using AttendanceUserManagementSystem.API.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BranchRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> AddBranch(Branch branch)
        {
            try
            {
               _applicationDbContext.Branches.Add(branch);
                _applicationDbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to add branch : " + ex.Message);

                return false;
                
            }
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            try
            {
                return await _applicationDbContext.Branches.Where(u => u.BranchName != "initial branch").ToListAsync();
            }
            catch (Exception ex)
            {

                Log.Error("Failed to get branches : " + ex);

                return null;
            }
        }

        public async Task<Branch> GetBranchById(int id)
        {
            try
            {
                return await _applicationDbContext.Branches.FirstOrDefaultAsync(u => u.BranchId == id);
            }
            catch (Exception ex)
            {

                Log.Error("Failed to get Branch with id " + id + ":" + ex.Message);

                return null;
            }
        }

        public async Task<bool> UpdateBranch(Branch branch)
        {
            try
            {
                _applicationDbContext.Branches.Update(branch);
                _applicationDbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                Log.Error("Failed to update Branch :" + ex.Message);

                return false;
            }
        }
    }
}
