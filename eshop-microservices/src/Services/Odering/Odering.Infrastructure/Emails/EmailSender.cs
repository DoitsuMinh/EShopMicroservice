using MailKit.Net.Smtp;
using MimeKit;
using Ordering.Application.Configuration.Emails;

namespace Odering.Infrastructure.Emails;

public class EmailSender : IEmailSender
{
    private readonly EmailsSettings _emailsSettings;
    public EmailSender(EmailsSettings emailsSettings)
    {
        _emailsSettings = emailsSettings;
    }
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        // Integration with email service
        // use Mailkit
        await Task.Delay(500); // Replace Thread.Sleep with an asynchronous delay

        //var message = new MimeMessage();
        //message.From.Add(new MailboxAddress(_emailsSettings.SenderName, emailMessage.From ?? _emailsSettings.SenderEmail));
        //message.To.Add(MailboxAddress.Parse(emailMessage.To));
        //message.Subject = emailMessage.Subject;

        //message.Body = new TextPart("html")
        //{
        //    Text = emailMessage.Content
        //};

        //using var client = new SmtpClient();
        //await client.ConnectAsync(_emailsSettings.SmtpServer, _emailsSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        //await client.AuthenticateAsync(_emailsSettings.SenderEmail, _emailsSettings.Password);
        //await client.SendAsync(message);
        //await client.DisconnectAsync(true);

        //return;
    }
}
