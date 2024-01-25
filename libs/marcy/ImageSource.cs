using System.Text.Json.Serialization;

using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public sealed record ImageSource : MediaSource<ImageMeta>
{
    public ImmutableList2<SizesItem> Sizes { get; init; } = ImmutableList2<SizesItem>.Empty;

    [JsonConstructor]
    public ImageSource(ImmutableList2<FileSrc<ImageMeta>> srcSet)
        : base(srcSet)
    {
    }

    public ImageSource(IEnumerable<FileSrc<ImageMeta>> srcSet, IEnumerable<SizesItem>? sizes = null)
       : this
        (
            srcSet as ImmutableList2<FileSrc<ImageMeta>> ?? srcSet.ToImmutableList2()
        )
    {
        Sizes = sizes as ImmutableList2<SizesItem> ?? sizes?.ToImmutableList2() ?? ImmutableList2<SizesItem>.Empty;
    }
}
