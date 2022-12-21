using Microsoft.AspNetCore.Identity;

namespace AttendanceUserManagementSystem.API.Authentication
{
    public class ApplicationUser : IdentityUser

    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeCode { get; set; }
        public string MacAddress { get; set; }
        public string IPAddress { get; set; }
        public bool ActivationStatus { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
