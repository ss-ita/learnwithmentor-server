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
        public static void SendEmail(string destination, string subject, string body)//make asynk
        {
            // настройка логина, пароля отправителя
            var from = "mr.dev.needs.acc@gmail.com";
            var pass = "maksymyshynDDR";

            // адрес и порт smtp-сервера, с которого мы и будем отправлять письмо
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);//465 - not respond

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.EnableSsl = true;

            // создаем письмо: message.Destination - адрес получателя
            var mail = new MailMessage(from, destination);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            client.SendMailAsync(mail);
        }
    }
}