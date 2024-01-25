using System.Text.Json.Serialization;

using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public sealed record Video : PixMedia
{
    public const string MediaType = "video";

    public override ImmutableList2<VideoSource> Sources { get; }

    public override string Type { get; } = MediaType;

    [JsonConstructor]
    public Video(ImmutableList2<VideoSource> sources)
    {
        Sources = sources;
    }

    public Video(IEnumerable<VideoSource> sources)
        : this(sources as ImmutableList2<VideoSource> ?? sources.ToImmutableList2())
    {

    }
}
