namespace CandyKingdom.Marcy;

public sealed record ImageMeta : PixMeta
{
    public static new ImageMeta Empty { get; } = new ImageMeta
    {
        Width = 0,
        Height = 0,
        ByteCount = 0
    };
}
