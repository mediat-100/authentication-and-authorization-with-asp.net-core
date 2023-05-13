using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WebApp.Settings;

namespace WebApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SmtpSetting> smtpSetting;

        public EmailService(IOptions<SmtpSetting> smtpSetting)
        {
            this.smtpSetting = smtpSetting;
        }
        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from,
               to,
               subject,
               body);

            using (var emailClient = new SmtpClient("smtp-relay.sendinblue.com", 587))
            {
                emailClient.Credentials = new NetworkCredential("yusuftomiwa740@gmail.com", "kOdHfvMLsb1Gr7VF");

                await emailClient.SendMailAsync(message);
            }
        }
    }
}
