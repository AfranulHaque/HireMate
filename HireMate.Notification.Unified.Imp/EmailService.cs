using System.Net.Mail;
using System.Net;
using System.Text;
using HireMate.Notification.Unified.Interface;
using HireMate.Common;
using Microsoft.Extensions.Options;
using HireMate.DataManagement.Repositories;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HireMate.Notification.Unified.Imp
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;
        public readonly IEventRepository _eventRepository;
        public readonly string  _pass;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
            _pass= Environment.GetEnvironmentVariable("Pass");
        }

        public async Task SendEmailWithIcsAsync(string toEmail, string subject, string body, string icsContent = null)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.User),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                if (!string.IsNullOrEmpty(icsContent))
                {
                    byte[] icsBytes = Encoding.UTF8.GetBytes(icsContent);
                    var icsAttachment = new Attachment(new MemoryStream(icsBytes), "invite.ics", "text/calendar");

                    mailMessage.Attachments.Add(icsAttachment);
                }


                //using var smtpClient = new SmtpClient(_settings.Host, _settings.Port)
                //{
                //    Credentials = new NetworkCredential(_settings.User, _settings.Pass),
                //    EnableSsl = true
                //};

                using var smtpClient = new SmtpClient(_settings.Host, _settings.Port)
                {
                    Credentials = new NetworkCredential(_settings.User, _pass),
                    EnableSsl = true, // or false depending on your SMTP server
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                await smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
