using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.Marcy.Pages;

public sealed record View : IHaveSkeleton
{
    public required string Code { get; init; }
    public required ImmutableList2<Bone> Bones { get; init; }
}
