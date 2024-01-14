using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CandyKingdom.MarcyCms.Sample;

public sealed class FakeEmailSender : IEmailSender
{
    public IReadOnlyList<string> Emails => _emails;

    private readonly List<string> _emails = [];

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailBuilder = new StringBuilder();

        emailBuilder.AppendLine(email).AppendLine(subject).AppendLine(htmlMessage);

        _emails.Add(emailBuilder.ToString());

        Console.WriteLine("New Email:");
        Console.WriteLine(emailBuilder);

        return Task.CompletedTask;
    }
}
