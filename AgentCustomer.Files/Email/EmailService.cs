
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace AgentCustomer.Files
{
    /// <summary>
    /// this could be a standalone micro service that handles notification of different types
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;
        private readonly EmailSettings _appSettings;
        public EmailService(IOptions<EmailSettings> appSettings, ILogger<EmailService> logger)
        {
            _logger = logger;
            _appSettings = appSettings.Value;

        }
        public void SendNotification(string to, string subject, string html, string from = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);

        }  
    }
}
