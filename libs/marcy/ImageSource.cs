using System.Collections.Immutable;

namespace CandyKingdom.Marcy;

public sealed record ImageSource : MediaSource<ImageMeta>
{
    public string MediaQuery { get; init; } = "";

    public ImmutableList<SizesItem> Sizes { get; init; } = ImmutableList<SizesItem>.Empty;

    public ImageSource(IEnumerable<FileSrc<ImageMeta>> srcSet, IEnumerable<SizesItem>? sizes = null)
      : base(srcSet)
    {
        Sizes =
          sizes as ImmutableList<SizesItem>
          ?? sizes?.ToImmutableList()
          ?? ImmutableList<SizesItem>.Empty;
    }
}
