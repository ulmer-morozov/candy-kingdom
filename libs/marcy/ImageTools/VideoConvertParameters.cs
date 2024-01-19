namespace CandyKingdom.Marcy.ImageTools;

public sealed record VideoConvertParameters
{
    public bool RemoveAudio { get; init; }

    public static VideoConvertParameters Default { get; } = new();
}
