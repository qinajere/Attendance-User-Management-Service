namespace AttendanceUserManagementSystem.API.Resources.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset Expiration { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
