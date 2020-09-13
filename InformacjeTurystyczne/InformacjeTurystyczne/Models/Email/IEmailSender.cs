using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
        void SendEmail(Message message);
    }
}
