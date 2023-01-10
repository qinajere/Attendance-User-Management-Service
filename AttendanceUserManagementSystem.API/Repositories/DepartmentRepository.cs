using AttendanceUserManagementSystem.API.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DepartmentRepository(ApplicationDbContext applicationDbContext)
        {      
                _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> AddDepartment(Department department)
        {
            try
            {
                _applicationDbContext.Departments.Add(department);
                _applicationDbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to add department : " + ex.Message);

                return false;

            }
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            try
            {
                return await _applicationDbContext.Departments.Where(u => u.DepartmentName != "initial department").ToListAsync();
            }
            catch (Exception ex)
            {

                Log.Error("Failed to get departments : " + ex);

                return null;
            }
        }

        public async Task<Department> GetDepartmentByID(int id)
        {
            try
            {
                return await _applicationDbContext.Departments.FirstOrDefaultAsync(u => u.DepartmentId == id);
            }
            catch (Exception ex)
            {

                Log.Error("Failed to get department with id " + id + ":" + ex.Message);

                return null;
            }
        }

        public async Task<bool> UpdateDepartment(Department department)
        {
            try
            {
                _applicationDbContext.Departments.Update(department);
                _applicationDbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                Log.Error("Failed to update department :" + ex.Message);

                return false;
            }
        }
    }
}
