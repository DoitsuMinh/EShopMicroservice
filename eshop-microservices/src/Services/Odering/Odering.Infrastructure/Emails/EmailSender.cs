using Ordering.Application.Configuration.Emails;

namespace Odering.Infrastructure.Emails;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(EmailMessage message)
    {
        // Integration with email service.

        return;
    }
}
