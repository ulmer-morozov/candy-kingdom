using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms;

public sealed record EmailSenderConfig : ConfigurationBase
{
    public int Port { get; init; }
    public string Host { get; init; } = "";
    public int Timeout { get; init; }

    public string SenderName { get; init; } = "";
    public string SenderEmail { get; init; } = "";
    public string UserLogin { get; init; } = "";
    public string UserPassword { get; init; } = "";

    public override void Verify()
    {
        ThrowIfEmpty(Port, $"{nameof(Port)} can not be 0");
        ThrowIfNullOrWhiteSpace(Host, $"{nameof(Host)} can not empty");
        ThrowIfNullOrWhiteSpace(SenderEmail, $"{nameof(SenderEmail)} can not empty");
        ThrowIfNullOrWhiteSpace(SenderName, $"{nameof(SenderName)} can not empty");
        ThrowIfNullOrWhiteSpace(UserLogin, $"{nameof(SenderName)} can not empty");
        ThrowIfNullOrWhiteSpace(UserPassword, $"{nameof(UserPassword)} can not empty");
    }

    public static EmailSenderConfig CreateFromEnv()
    {
        var config = new EmailSenderConfig
        {
            Port = GetEnvVarAsIntOrThrow("SMTP_PORT"),
            Timeout = GetEnvVarAsIntOrThrow("SMTP_TIMEOUT"),
            Host = GetEnvVarOrThrow("SMTP_HOST"),
            UserLogin = GetEnvVarOrThrow("SMTP_LOGIN"),
            UserPassword = GetEnvVarOrThrow("SMTP_PASSWORD"),
            SenderName = GetEnvVarOrThrow("SMTP_SENDER_NAME"),
            SenderEmail = GetEnvVarOrThrow("SMTP_SENDER_EMAIL"),
        };

        config.Verify();

        return config;
    }
}
