using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.Marcy.Pages;

public sealed record View : IHaveSkeleton
{
    public Guid Id { get; init; }
    public string Code { get; init; } = "";
    public ImmutableList2<Bone> Bones { get; init; } = ImmutableList2<Bone>.Empty;
}
