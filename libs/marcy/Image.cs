using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public sealed record Image : PixMedia
{
    private const string IMAGE_MEDIA_TYPE = "image";

    public override ImmutableList2<ImageSource> Sources { get; }

    public override string Type { get; } = IMAGE_MEDIA_TYPE;

    private Image(ImmutableList2<ImageSource> sources)
      : base(sources)
    {
        Sources = sources;
    }

    public Image(IEnumerable<ImageSource> sources)
      : this(sources as ImmutableList2<ImageSource> ?? sources.ToImmutableList2()) { }
}
