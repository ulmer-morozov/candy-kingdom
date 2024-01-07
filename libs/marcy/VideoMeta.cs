namespace CandyKingdom.Marcy;

public sealed record VideoMeta : PixMeta
{
    public required TimeSpan Duration { get; init; }

    public required string FullFormat { get; init; }

    public required bool HasAudio { get; init; }

    public double FrameRate { get; init; }
}
