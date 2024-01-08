using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.Marcy.Pages;

public sealed record Page : IHaveSkeleton
{
    public required string Route { get; init; }
    public required LocalizedString Title { get; init; }
    public required ImmutableList2<Bone> Bones { get; init; }
    public required ImmutableList2<Page> Children { get; init; }

    public Page Clean(bool removeBones = false, bool removeChildren = false)
    {
        var result = this;

        if (removeBones && !Bones.IsEmpty)
        {
            result = result with { Bones = ImmutableList2<Bone>.Empty };
        }

        if (!Children.IsEmpty)
        {
            if (removeChildren)
            {
                result = result with { Children = ImmutableList2<Page>.Empty };
            }
            else
            {
                result = result with
                {
                    Children = Children
                        .Select(x => x.Clean(removeBones: removeBones))
                        .ToImmutableList2()
                };
            }
        }

        return result;
    }
}
