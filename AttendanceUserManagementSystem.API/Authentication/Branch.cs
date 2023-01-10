namespace AttendanceUserManagementSystem.API.Authentication
{
    public class Branch
    {
        public int BranchId { get; set; }    
        public string BranchName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }


    }
}
