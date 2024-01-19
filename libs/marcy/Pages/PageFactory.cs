using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy.Pages;

public abstract class PageFactory : SkeletonFactory
{
    public abstract string Route { get; }
    public abstract LocalizedString Title { get; }
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

    public Page Create()
    {
        var children = Children.Select(x => x.Create()).ToImmutableList2();

        var page = new Page
        {
            Route = Route,
            Title = Title,
            Bones = Bones.ToImmutableList2(),
            Children = children
        };

        return page;
    }
}
