using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public sealed record LinksBone : Bone
{
    public required LocalizedString Title { get; init; }
    public required LocalizedString Text { get; init; }
    public required ImmutableList2<WebLink> Links { get; init; }

    public LinksBone(IEnumerable<WebLink> links)
        : this()
    {
        Links =
            links as ImmutableList2<WebLink>
            ?? links?.ToImmutableList2()
            ?? ImmutableList2<WebLink>.Empty;
    }

    public LinksBone()
        : base("links") { }
}
