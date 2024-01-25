using System.Text.Json.Serialization;

using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public sealed record Image : PixMedia
{
    public const string MediaType = "image";

    public override ImmutableList2<ImageSource> Sources { get; }

    public override string Type { get; } = MediaType;

    [JsonConstructor]
    public Image(ImmutableList2<ImageSource> sources)
    {
        Sources = sources;
    }

    public Image(IEnumerable<ImageSource> sources)
        : this(sources as ImmutableList2<ImageSource> ?? sources.ToImmutableList2())
    {

    }
}
