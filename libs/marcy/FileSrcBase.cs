namespace CandyKingdom.Marcy;

public record FileSrcBase
{
    public required string MimeType { get; init; }

    public required string Url { get; init; }
}
