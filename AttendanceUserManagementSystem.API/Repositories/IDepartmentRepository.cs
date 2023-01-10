using AttendanceUserManagementSystem.API.Authentication;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public interface IDepartmentRepository
    {
        Task<bool> AddDepartment(Department department);
        Task<bool> UpdateDepartment(Department department);
        Task<Department> GetDepartmentByID (int id);
        Task<List<Department>> GetAllDepartments();
    }
}
