using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.Marcy.Pages;

public interface IHaveSkeleton
{
    ImmutableList2<Bone> Bones { get; }
}
