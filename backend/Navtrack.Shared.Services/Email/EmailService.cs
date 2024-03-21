using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Services.Email.Emails;
using Navtrack.Shared.Services.Settings;
using Navtrack.Shared.Services.Settings.Models;

namespace Navtrack.Shared.Services.Email;

[Service(typeof(IEmailService))]
public class EmailService(ISettingService service) : IEmailService
{
    public async Task Send(string destination, IEmail email)
    {
        EmailSettings? emailSettings = await service.Get<EmailSettings>();

        if (!string.IsNullOrEmpty(emailSettings?.SmtpServer) && 
            !string.IsNullOrEmpty(emailSettings.SmtpUsername) &&
            !string.IsNullOrEmpty(emailSettings.SmtpPassword))
        {
            SmtpClient smtpClient = new(emailSettings.SmtpServer)
            {
                Port = emailSettings.SmtpPort,
                Credentials = new NetworkCredential(emailSettings.SmtpUsername, emailSettings.SmtpPassword),
                EnableSsl = true
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(emailSettings.SmtpFrom),
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true,
                To = { destination }
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}