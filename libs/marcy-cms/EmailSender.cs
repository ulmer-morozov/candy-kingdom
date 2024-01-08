using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CandyKingdom.MarcyCms;

public sealed class EmailSender : IEmailSender
{
    private readonly EmailSenderConfig _config;

    public EmailSender(EmailSenderConfig config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var client = new SmtpClient
        {
            Host = _config.Host,
            Port = _config.Port,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true,
            Credentials = new NetworkCredential(_config.UserLogin, _config.UserPassword),
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config.SenderEmail, _config.SenderName),
            Body = htmlMessage,
            Subject = subject,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}
