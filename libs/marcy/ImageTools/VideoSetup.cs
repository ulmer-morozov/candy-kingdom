using System.Drawing;

namespace CandyKingdom.Marcy.ImageTools;

public sealed record VideoSetup
{
    public Size Size { get; init; }
    public VideoFormat Format { get; init; } = VideoFormat.Empty;

    public static VideoSetup Empty { get; } = new();
}
