namespace AttendanceUserManagementSystem.API.Resources.Models
{
    public class ChangePasswordModel
    {
        public string UserID { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
