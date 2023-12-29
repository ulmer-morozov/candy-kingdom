namespace CandyKingdom.Marcy.ImageTools;

public record FileFormat
{
    public required string Extension { get; init; }
    public required string MimeType { get; init; }

    public static FileFormat Empty { get; } = new FileFormat { Extension = "", MimeType = "" };
}
