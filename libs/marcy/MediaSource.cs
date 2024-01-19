using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public record MediaSource<TMeta> : MediaSourceBase
  where TMeta : FileMeta
{
    public override ImmutableList2<FileSrc<TMeta>> SrcSet { get; }

    private MediaSource(ImmutableList2<FileSrc<TMeta>> srcSet)
      : base(srcSet)
    {
        SrcSet = srcSet;
    }

    public MediaSource(IEnumerable<FileSrc<TMeta>> srcSet)
      : this(srcSet as ImmutableList2<FileSrc<TMeta>> ?? srcSet.ToImmutableList2()) { }
}
