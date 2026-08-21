using Notification.ServicesAbstract;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using Notification.Settings;
namespace Notification.Service
{
    public  class EmailService : IEmailService
    {

        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, IList<IFormFile> attachment = null)
        {
            Console.WriteLine($"To Email: '{toEmail}'");

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(toEmail));
            var builder = new BodyBuilder();
            if (attachment != null)
            {
                Byte[] fileBytes;
                foreach (var file in attachment)
                {
                    if (file.Length > 0)
                    {
                        using var stream = new MemoryStream();
                        file.CopyTo(stream);
                        fileBytes = stream.ToArray();
                        builder.Attachments.Add(file.FileName, stream.ToArray(), ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.Username, _mailSettings.Email));
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);


        }
    }
}

