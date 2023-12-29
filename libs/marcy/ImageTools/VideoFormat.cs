namespace CandyKingdom.Marcy.ImageTools;

public sealed record VideoFormat : FileFormat
{
  public bool IsEmpty => Equals(Empty);

  public static new VideoFormat Empty { get; } = new VideoFormat { Extension = "", MimeType = "" };

  public static VideoFormat Mp4 { get; } =
    new VideoFormat { Extension = ".mp4", MimeType = "video/mp4" };

  public static VideoFormat WebM { get; } =
    new VideoFormat { Extension = ".webm", MimeType = "video/webm" };
}
