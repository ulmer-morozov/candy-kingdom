using System.Drawing;

namespace CandyKingdom.Marcy.ImageTools;

public sealed record VideoSetup
{
    public required Size Size { get; init; }
    public required VideoFormat Format { get; init; }
}
