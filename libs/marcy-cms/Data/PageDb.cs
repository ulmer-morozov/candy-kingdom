using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Data;

public sealed class PageDb : JsonDataOwner<PageData>
{
    public Guid Id { get; private set; }
    public LocalizedString Title { get; private set; }
    public OpenGraphData OG { get; init; }
    public ImmutableList2<Bone> Bones { get; private set; }

    public PageDb? Parent { get; private set; }

    public PageDb(Guid id, LocalizedString title, OpenGraphData og, IEnumerable<Bone> bones)
    {
        Id = id;
        Title = title;
        OG = og;

        Bones = bones as ImmutableList2<Bone>
                            ?? bones?.ToImmutableList2()
                            ?? ImmutableList2<Bone>.Empty;
    }

    private PageDb()
    {
        Title = null!;
        OG = null!;
        Bones = null!;
    }
}
