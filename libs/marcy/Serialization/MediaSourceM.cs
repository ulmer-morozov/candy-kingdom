namespace CandyKingdom.Marcy.Serialization;

public sealed record MediaSourceM
{
    public required string Path { get; init; }
    public string? MediaQuery { get; init; }

    public MediaSourceM() { }

    public static MediaSourceM New(string path, string? mediaQuery = null)
    {
        return new() { Path = path, MediaQuery = mediaQuery };
    }
}
