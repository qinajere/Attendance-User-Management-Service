using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AttendanceUserManagementSystem.API.Resources.ResourceParameters;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllUsers(GetUsersResourceParameters parameters);
        Task<List<string>> GetAllRoles();
        Task<ApplicationUser> GetUserById(string id);
        Task AddUser(RegisterUserDto user);
        Task UpdateUser(ApplicationUser user);
       
        Task<string> CreateUsername(string firstname, string lastname);
        Task<bool> InitAdmin();

    }
}
