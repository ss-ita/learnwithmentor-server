using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Mail;

namespace LearnWithMentor.Services
{
    public static class EmailService
    {
        public static Task SendEmail(string destination, string subject, string body)
        {
            var from = "mr.dev.needs.acc@gmail.com";
            var pass = "maksymyshynDDR";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.EnableSsl = true;

            var mail = new MailMessage(from, destination);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            return client.SendMailAsync(mail);
        }
    }
}