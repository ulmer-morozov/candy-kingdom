using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.Marcy.Pages;

public sealed record Page<T>
    where T : PageData, new()
{
    public required T Data { get; init; }
}

public record Page : IHaveSkeleton
{
    public string Route { get; init; } = "";
    public LocalizedString Title { get; init; } = new();
    public OpenGraphData OG { get; init; } = new();
    public ImmutableList2<Bone> Bones { get; init; } = ImmutableList2<Bone>.Empty;
    public ImmutableList2<Page> Children { get; init; } = ImmutableList2<Page>.Empty;

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
