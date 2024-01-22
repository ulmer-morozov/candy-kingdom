using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy.Pages;

public abstract class PageFactory<T> : PageFactory
    where T : PageData, new()
{
    public abstract T Data { get; }

    public override Page Create(string baseUrl)
    {
        var page = base.Create(baseUrl);

        var newPage = new Page<T>(page)
        {
            Data = Data
        };

        return newPage;
    }
}

public abstract class PageFactory : SkeletonFactory
{
    public PublishStatus PublishStatus { get; } = PublishStatus.Published;
    public abstract string Route { get; }
    public abstract LocalizedString Title { get; }
    public abstract OpenGraphData OpenGraph { get; }
    public List<PageFactory> Children { get; set; } = [];

    public PageFactory AddChild<T>(T child)
        where T : PageFactory
    {
        Children.Add(child);

        return this;
    }

    public PageFactory AddChild<T>(Action<T>? instanceAction = null)
        where T : PageFactory, new()
    {
        var instance = new T();

        instanceAction?.Invoke(instance);

        Children.Add(instance);

        return this;
    }

    public virtual Page Create(string baseUrl)
    {
        var pageUrl = baseUrl == "" && Route == WebsiteRoot.RootPrefix
                ? WebsiteRoot.RootPrefix
                : $"{baseUrl}{Route}";

        var passedBaseUrl = pageUrl == WebsiteRoot.RootPrefix
                ? "/"
                : $"{pageUrl}/";

        var children = Children
            .Select(x => x.Create(passedBaseUrl))
            .Select((x, i) => x with { Order = i })
            .ToImmutableList2();

        var page = new Page
        {
            Url = pageUrl,
            Route = Route,
            PublishStatus = PublishStatus,
            Title = Title,
            OpenGraph = OpenGraph,
            Bones = Bones.ToImmutableList2(),
            Children = children
        };

        return page;
    }
}
