﻿namespace AttendanceUserManagementSystem.API.Resources.Models
{
    public class EmailMessage
    {

        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
