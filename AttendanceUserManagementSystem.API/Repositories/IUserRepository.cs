using AttendanceUserManagementSystem.API.Authentication;
using AttendanceUserManagementSystem.API.Resources.DTO;
using AttendanceUserManagementSystem.API.Resources.ResourceParameters;
using AttendanceUserManagementSystem.API.Resources.Responses;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllUsers(GetUsersResourceParameters parameters);
        Task<List<string>> GetAllRoles();
        Task<ApplicationUser> GetUserById(string id);
        Task<ApplicationUser> GetUserByCode(string code);
        Task AddUser(RegisterUserDto user);
        Task<bool> AddRangeAddresses(List<ApplicationUser> users);
        Task UpdateUser(ApplicationUser user);
       
        Task<string> CreateUsername(string firstname, string lastname);
        Task<bool> InitAdmin();

    }
}
