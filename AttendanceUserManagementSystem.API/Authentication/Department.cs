namespace AttendanceUserManagementSystem.API.Authentication
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
