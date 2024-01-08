using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.Marcy.Pages;

public sealed record View : IHaveSkeleton
{
    public required string Id { get; init; }
    public required ImmutableList2<Bone> Bones { get; init; }
}
