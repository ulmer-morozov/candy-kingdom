namespace CandyKingdom.Marcy;

public record FileMeta
{
    public required long ByteCount { get; init; }

    public static FileMeta Empty { get; } = new FileMeta { ByteCount = 0 };
}
