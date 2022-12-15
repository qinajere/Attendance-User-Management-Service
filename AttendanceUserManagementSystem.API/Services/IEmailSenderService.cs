using AttendanceUserManagementSystem.API.Resources.Models;



namespace AttendanceUserManagementSystem.API.Services
{
    public interface IEmailSenderService
    {
        void SendEmail(EmailMessage message);
    }
}
