namespace AttendanceUserManagementSystem.API.Resources.DTO
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeCode { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public bool AddressAuthenticationExemption { get; set; }

    }
}
