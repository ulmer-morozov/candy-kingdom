using System.Drawing;

namespace CandyKingdom.Marcy.ImageTools;

public sealed record ImageSetup
{
  public required Size Size { get; init; }
  public required ImageFormat Format { get; init; }
  public ImageTransformType Type { get; init; }
}
