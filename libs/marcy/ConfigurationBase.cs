namespace CandyKingdom.Marcy;

public abstract record ConfigurationBase
{
    public abstract void Verify();

    protected void ThrowIfEmpty(int value, string message)
    {
        ThrowIf(value == 0, message);
    }

    protected void ThrowIfNullOrWhiteSpace(string value, string message)
    {
        ThrowIf(string.IsNullOrWhiteSpace(value), message);
    }

    protected void ThrowIf(bool condition, string message)
    {
        if (condition)
        {
            throw new Exception($"{GetType().Name}Configuration Error: {message})");
        }
    }

    protected static string GetEnvVarOrThrow(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Required environment variable {name} is not set");
    }

    protected static int? GetEnvVarAsInt(string name)
    {
        var timeoutString = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        if (!int.TryParse(timeoutString, out var timeout))
        {
            throw new Exception($"Can not parse int value for environment variable {name} = {timeoutString}.");
        }

        return timeout;
    }

    protected static int GetEnvVarAsIntOrThrow(string name)
    {
        return GetEnvVarAsInt(name) ?? throw new Exception($"Required environment variable {name} is not set");
    }
}
