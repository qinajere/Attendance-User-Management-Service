namespace AttendanceUserManagementSystem.API.Resources.ResourceParameters
{
    public class GetUsersResourceParameters
    {
        public bool? ActivationStatus { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public string? Role { get; set; }
        public string? EmployeeCode { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
