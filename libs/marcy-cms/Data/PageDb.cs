using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Data;

public sealed class PageDb
{
    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public string Route { get; private set; }
    public int Order { get; private set; }
    public PublishStatus PublishStatus { get; private set; }
    public LocalizedString Title { get; private set; }
    public OpenGraphData OpenGraph { get; private set; }
    public ImmutableList2<Bone> Bones { get; private set; }

    public PageData Data { get; private set; }

    public PageDb? Parent { get; private set; }
    public ICollection<PageDb> Children { get; private set; }

    public PageDb(Guid id, string url, string route, int order, PublishStatus publishStatus, LocalizedString title, OpenGraphData openGraph, IEnumerable<Bone> bones, PageData? data = null, PageDb? parent = null, IEnumerable<PageDb>? children = null)
    {
        Id = id;
        Url = url;
        Title = title;
        Order = order;
        OpenGraph = openGraph;
        Route = route;
        PublishStatus = publishStatus;

        Data = data ?? PageData.Empty;

        Bones = bones as ImmutableList2<Bone>
                            ?? bones?.ToImmutableList2()
                            ?? ImmutableList2<Bone>.Empty;

        Parent = parent;
        Children = children?.ToList() ?? [];
    }

    private PageDb()
    {
        Url = null!;
        Title = null!;
        Route = null!;
        Data = null!;
        OpenGraph = null!;
        Bones = null!;
        Children = null!;
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
