using System.Collections.Immutable;

namespace CandyKingdom.Marcy;

public sealed record VideoSource : MediaSource<VideoMeta>
{
    public string MediaQuery { get; init; } = "";

    public VideoSource(IEnumerable<FileSrc<VideoMeta>> srcSet)
      : base(srcSet) { }

    public VideoSource()
      : this(ImmutableList<FileSrc<VideoMeta>>.Empty) { }
}
