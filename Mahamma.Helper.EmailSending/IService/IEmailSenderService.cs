using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Helper.EmailSending.IService
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmail(string subject, string body, string toEmail);
    }
}
