using System.ComponentModel.DataAnnotations;

namespace Ordering.Application.Configuration.Emails;

public class EmailsSettings
{
    //public string FromAddressEmail { get; set; } = string.Empty;
    [Required]
    public string SmtpServer { get; set; } = string.Empty;
    [Required]
    public int SmtpPort { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
