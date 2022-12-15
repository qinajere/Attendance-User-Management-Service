using System.Net.Mail;
using System.Net;
using AttendanceUserManagementSystem.API.Resources.Models;

namespace AttendanceUserManagementSystem.API.Services
{
    public class EmailSenderService:IEmailSenderService
    {
        IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(EmailMessage message)
        {



            try
            {
                MailMessage mailMessage = new MailMessage(message.From, message.To);
                mailMessage.Subject = message.Subject;
                mailMessage.Body = message.Content;
                mailMessage.IsBodyHtml = true;

                //var client = new SmtpClient("172.16.254.107", 25)

                var client = new SmtpClient(_configuration.GetSection("Constants").GetSection("EmailServerAddress").Value)
                {
                    Credentials = new NetworkCredential(_configuration.GetSection("Constants").GetSection("EmailName").Value, _configuration.GetSection("Constants").GetSection("EmailPassword").Value),
                    EnableSsl = false,
                    UseDefaultCredentials = true,



                };


               client.Send(mailMessage);
            }
            catch (Exception Ex)
            {

                throw new Exception($"Exception {Ex.InnerException} Message {Ex.Message}");
            } 
            
        }
    }
}
