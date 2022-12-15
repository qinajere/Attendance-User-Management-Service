
using AttendanceUserManagementSystem.API.Resources.Models;

namespace AttendanceUserManagementSystem.API.Helpers
{
    public static class RegistrationEmailContent
    {
      
        public static string PasswordEmailContent(PasswordEmailModel passwordEmailModel, IConfiguration configuration)
        {
            var content = "<!DOCTYPE html>" +
                "<html lang=\"en\">" +
                "<head>" +
                    "<meta charset=\"UTF-8\">" +
                    " <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">" +
                    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                "</head>" +
                "<body>" +
                "<div class=\"container\" style=\"width: 100%; height: 100%;\">" +
                "<center>"+
     
                "<div class=\"card\" style=\"margin-top: 4rem; width: 800px; height: 500px; background: #ffffff; margin: auto; padding: 2rem; border-radius: 10px; max-width: 100%; display: flex; justify-content: center; flex-direction: column; align-items: center; \">" +
                "<div class=\"logo\" style=\"width: 50px; height:50px;\">" +
                       " <img src=\"http://10.3.0.73/pha/logo.svg\" alt=\"\" srcset=\"\">" +
                "</div>" +
                " <div class=\"header\" style=\"width: 500px; text-align: center;\">" +
                     "<h1 style=\"text-align: center; margin-top: 1rem; font-size: 2rem; line-height: 2rem; font-weight: 700;\">" +
                     "Your Attendance Account has been registered successfully." +
                     "</h1>" +
                "</div>" +
                "<div class=\"text\" style=\"margin-top: 1rem; width: 600px; text-align: center;\">" +
                     "<p>" +
                     "Hi "+ passwordEmailModel.FirstName + "  " + passwordEmailModel.LastName + " Welcome to NITEL Attendance System. To get started, login using the credentials provided below." +
                     "</p>" +
                "</div>" +
                "<div class=\"table\" style=\"margin-top: .5rem; width:100%\">" +
                "<center>" +
                     "<table style=\"width: 400px; border: 1px solid #ebebeb;\">" +
                          "<thead>" +
                                "<tr>" +
                                    "<th style=\"padding: 10px; background-color: #10988d; color: #fff; text-align: left;\">Username</th>" +
                                    "<th style=\"padding: 10px; background-color: #10988d; color: #fff; text-align: left;\">Password</th>" +
                                    "<th style=\"padding: 10px; background-color: #10988d; color: #fff; text-align: left;\">Role</th>" +
                                "</tr>" +
                          "</thead>" +
                          "<tbody>" +
                                "<tr>" +
                                    "<td style=\"padding: 10px; border: 1px solid #ebebeb;\">" + passwordEmailModel.UserName  + "</td>" +
                                    "<td style=\"padding: 10px; border: 1px solid #ebebeb;\">" + passwordEmailModel.Password + "</td>" +
                                    "<td style=\"padding: 10px; border: 1px solid #ebebeb;\">" + passwordEmailModel.Role + "</td>" +
                                "</tr>" +
                          "</tbody>" +
                     "</table>" +
                     "</center>"+
                "</div>" +
                "<div>" +
                      "<p>" +
                            "<a href=\""+ configuration.GetSection("Constants").GetSection("AttendanceLink").Value + "\">Click here to login to PHA</a>" +
                      "</p>" +
                "</div>" +
                "<div class=\"link\" style=\"margin-top: 1rem;\">" +
                    "<p>PHA by Fintech</p>" +
                "</div>" +
           "</div>" +
           "</center>"+
          "</div>" +
         "</body>" +
         "</html>";

                                             

            return content;
        }
    }
}
