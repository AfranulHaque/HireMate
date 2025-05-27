using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ical.Net.CalendarComponents;

namespace HireMate.Notification.Unified.Interface
{
    public interface IEmailService
    {
        Task SendEmailWithIcsAsync(string toEmail, string subject, string body, string icsContent);
    }
}
