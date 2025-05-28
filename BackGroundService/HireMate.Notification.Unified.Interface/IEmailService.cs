namespace HireMate.Notification.Unified.Interface
{
    public interface IEmailService
    {
        Task SendEmailWithIcsAsync(string toEmail, string subject, string body, string icsContent);
    }
}
