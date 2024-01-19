using System.Drawing;

namespace CandyKingdom.Marcy.ImageTools;

public sealed record ImageSetup
{
    public Size Size { get; init; }
    public ImageFormat Format { get; init; } = ImageFormat.Empty;
    public ImageTransformType Type { get; init; }
}
