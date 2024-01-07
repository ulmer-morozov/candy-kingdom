namespace CandyKingdom.Marcy;

public abstract class ConfigurationBase
{
    public abstract void Verify();

    protected static void ThrowIfEmpty(int value, string message)
    {
        ThrowIf(value == 0, message);
    }

    protected static void ThrowIfNullOrWhiteSpace(string value, string message)
    {
        ThrowIf(string.IsNullOrWhiteSpace(value), message);
    }

    protected static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new Exception($"Mailer ronfiguration Error: {message})");
    }
}
