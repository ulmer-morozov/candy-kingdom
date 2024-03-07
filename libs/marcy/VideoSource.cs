using System.Text.Json.Serialization;

using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public sealed record VideoSource : MediaSource<VideoMeta>
{
    [JsonConstructor]
    public VideoSource(ImmutableList2<FileSrc<VideoMeta>> srcSet)
        : base(srcSet.OrderByDescending(x => x.Meta.Width).ThenByDescending(x => x.MimeType.Contains("web")))
    {
    }

    public VideoSource(IEnumerable<FileSrc<VideoMeta>> srcSet)
        : this(srcSet as ImmutableList2<FileSrc<VideoMeta>> ?? srcSet.ToImmutableList2())
    {

    }
}
