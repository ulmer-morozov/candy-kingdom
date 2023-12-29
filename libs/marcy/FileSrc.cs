namespace CandyKingdom.Marcy;

public sealed record FileSrc<T> : FileSrcBase
  where T : FileMeta
{
  public required T Meta { get; init; }
}
