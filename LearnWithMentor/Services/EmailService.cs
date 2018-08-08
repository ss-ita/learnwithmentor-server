using System.Threading.Tasks;
using System.Net.Mail;
using LearnWithMentor.Filters;
using LearnWithMentor.Models;
using System.Configuration;
using System.Security.Cryptography;

namespace LearnWithMentor.Services
{
    public static class EmailService
    {
        public static Task SendEmail(string destination, string subject, string body)
        {
            var from = ConfigurationManager.AppSettings["BaseEmail"];
            var pass = ConfigurationManager.AppSettings["EmailPassword"]; 
            var SmtpHost = ConfigurationManager.AppSettings["SmtpClient"];
            var SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpClientPort"]);

            var client = new SmtpClient(SmtpHost, SmtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(@from, pass),
                EnableSsl = true
            };
            var mail = new MailMessage(from, destination)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            return client.SendMailAsync(mail);
        }

        public static Task SendConfirmPasswordEmail(string email, string token, string emailConfirmLink)
        {
            var callbackUrl = emailConfirmLink + "/" + token;
            return SendEmail(email, "Email confirmation",
                $"<h1><b>LearnWithMentor</b></h1> <p>Follow this link to confirm your email address: <a href=\"{callbackUrl}\">Confirm</a></p>");
        }

        public static Task SendPasswordResetEmail(string email, string token, string resetPasswordLink)
        {
            var callbackUrl = resetPasswordLink + "/" + token;
            return SendEmail(email, "Password reset",
                $"<h1><b>LearnWithMentor</b></h1> <p>Follow this link to reset your password: <a href=\"{callbackUrl}\">Reset password</a></p>");
        }
    }
}