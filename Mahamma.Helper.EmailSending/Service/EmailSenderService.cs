using Mahamma.Helper.EmailSending.IService;
using Mahamma.Helper.EmailSending.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Helper.EmailSending.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SMTPSettings _sMTPSettings;
        public EmailSenderService(SMTPSettings sMTPSettings)
        {
            _sMTPSettings = sMTPSettings;
        }
        public async Task<bool> SendEmail(string subject, string body, string toEmail)
        {
            bool sent = false;
            using (SmtpClient smtp = new SmtpClient())
            {
                using MailMessage message = new MailMessage()
                {
                    From = new MailAddress(_sMTPSettings.Username, _sMTPSettings.DisplayName),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body,
                };
                message.To.Add(toEmail);
                smtp.Timeout = _sMTPSettings.Timeout * 1000;
                smtp.Host = _sMTPSettings.Host;
                smtp.Port = _sMTPSettings.Port;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_sMTPSettings.Username, _sMTPSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.SendCompleted += (obj, e) =>
                {
                    if (!e.Cancelled || e.Error == null)
                    {
                        Console.WriteLine("Mail sent successfully.");
                    }
                };
                await smtp.SendMailAsync(message);
                sent = true;
                return sent;
            }
        }
    }
}
