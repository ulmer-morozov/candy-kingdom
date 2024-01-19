namespace CandyKingdom.Marcy.ImageTools;

public sealed record ImageFormat : FileFormat
{
    public bool IsEmpty => Equals(Empty);

    public static new ImageFormat Empty { get; } = new ImageFormat { Extension = "", MimeType = "" };

    public static ImageFormat Jpeg { get; } =
      new ImageFormat { Extension = ".jpg", MimeType = "image/jpeg" };
    public static ImageFormat Png { get; } =
      new ImageFormat { Extension = ".png", MimeType = "image/png" };
    public static ImageFormat WebP { get; } =
      new ImageFormat { Extension = ".webp", MimeType = "image/webp" };
}
