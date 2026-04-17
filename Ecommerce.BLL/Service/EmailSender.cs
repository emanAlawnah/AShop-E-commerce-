using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("Alawnah.eman@gmail.com", "iamb tali duno xkmd")
            };

            return client.SendMailAsync(
                new MailMessage(from: "Alawnah.eman@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml =true}
                );
        
    }
    }
}
