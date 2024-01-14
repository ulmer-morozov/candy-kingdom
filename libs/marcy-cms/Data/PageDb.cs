using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Data;

public sealed class PageDb : JsonDataOwnerDb<PageData>
{
    public string Url { get; private set; }
    public string Route { get; private set; }
    public int Order { get; private set; }
    public PublishStatus PublishStatus { get; private set; }
    public LocalizedString Title { get; private set; }
    public OpenGraphData OpenGraph { get; private set; }
    public ImmutableList2<Bone> Bones { get; private set; }

    public PageDb? Parent { get; private set; }
    public ICollection<PageDb> Childern { get; private set; }

    public PageDb(string url, string route, int order, PublishStatus publishStatus, LocalizedString title, OpenGraphData openGraph, IEnumerable<Bone> bones, PageDb? parent = null, IEnumerable<PageDb>? children = null)
    {
        Url = url;
        Title = title;
        Order = order;
        OpenGraph = openGraph;
        Route = route;
        PublishStatus = publishStatus;

        Bones = bones as ImmutableList2<Bone>
                            ?? bones?.ToImmutableList2()
                            ?? ImmutableList2<Bone>.Empty;

        Parent = parent;
        Childern = children?.ToList() ?? [];
    }

    private PageDb()
    {
        Url = null!;
        Title = null!;
        Route = null!;
        OpenGraph = null!;
        Bones = null!;
        Childern = null!;
    }

    public void Copy(Page page)
    {
        Order = page.Order;
        PublishStatus = page.PublishStatus;
        Title = page.Title;
        OpenGraph = page.OpenGraph;
        Bones = page.Bones;
    }
}
