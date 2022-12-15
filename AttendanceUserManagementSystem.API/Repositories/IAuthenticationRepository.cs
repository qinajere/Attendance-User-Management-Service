using AttendanceUserManagementSystem.API.Resources.Models;
using AttendanceUserManagementSystem.API.Resources.Responses;
using NuGet.Protocol.Plugins;

namespace AttendanceUserManagementSystem.API.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<LoginResponse> Login(LoginModel loginModel);
        Task<bool> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<bool> ChangePasswordAdmin(ChangePasswordAdminModel changePasswordAdminModel);
    }
}
