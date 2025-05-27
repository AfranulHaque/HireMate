using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HireMate.Notification.Unified.Interface;
using Ical.Net.CalendarComponents;
using HireMate.Common;
using Microsoft.Extensions.Options;
using HireMate.Common.Utils;

namespace HireMate.Notification.Unified.Imp
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailWithIcsAsync(string toEmail, string subject, string body, CalendarEvent calendarEvent)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.User),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            string icsContent = EmailHelper.CreateCalendarEntry(DateTime.Now, DateTime.Now, string.Empty, string.Empty, string.Empty, new List<string>());
            byte[] icsBytes = Encoding.UTF8.GetBytes(icsContent);
            var icsAttachment = new Attachment(new MemoryStream(icsBytes), "invite.ics", "text/calendar");

            mailMessage.Attachments.Add(icsAttachment);

            using var smtpClient = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.User, _settings.Pass),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}
