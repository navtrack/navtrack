using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Navtrack.Common.Email.Emails;
using Navtrack.Common.Settings;
using Navtrack.Common.Settings.Settings;
using Navtrack.Library.DI;

namespace Navtrack.Common.Email;

[Service(typeof(IEmailService))]
public class EmailService : IEmailService
{
    private readonly ISettingService settingService;

    public EmailService(ISettingService settingService)
    {
        this.settingService = settingService;
    }

    public async Task Send(string destination, IEmail email)
    {
        EmailSettings? emailSettings = await settingService.Get<EmailSettings>();

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