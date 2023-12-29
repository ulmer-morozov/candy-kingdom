using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public sealed record Video : PixMedia
{
  private const string VIDEO_MEDIA_TYPE = "video";

  public override ImmutableList2<VideoSource> Sources { get; }

  public override string Type { get; } = VIDEO_MEDIA_TYPE;

  private Video(ImmutableList2<VideoSource> sources)
    : base(sources)
  {
    Sources = sources;
  }

  public Video(IEnumerable<VideoSource> sources)
    : this(sources as ImmutableList2<VideoSource> ?? sources.ToImmutableList2()) { }
}
