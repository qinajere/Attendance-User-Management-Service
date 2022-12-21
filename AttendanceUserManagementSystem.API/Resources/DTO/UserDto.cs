namespace AttendanceUserManagementSystem.API.Resources.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string EmployeeCode { get; set; }
        public string Role { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public bool ActivationStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public bool AddressAuthenticationExemption { get; set; }
    }
}
