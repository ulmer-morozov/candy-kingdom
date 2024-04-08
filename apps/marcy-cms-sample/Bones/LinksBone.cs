using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Sample.Bones;

// todo: add  this bone
public sealed record LinksBone : Bone
{
    public const string BoneType = "links";
    public required LocalizedString Title { get; init; }
    public required LocalizedString Text { get; init; }
    public required ImmutableList2<WebLink> Links { get; init; }

    public override string Type { get; } = BoneType;

    public LinksBone(IEnumerable<WebLink> links)
    {
        Links =
            links as ImmutableList2<WebLink>
            ?? links?.ToImmutableList2()
            ?? ImmutableList2<WebLink>.Empty;
    }
}
