using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy.Pages;
public abstract class ViewFactory : SkeletonFactory
{
    public abstract string Id { get; }

    public View Create()
    {
        var view = new View { Id = Id, Bones = Bones.ToImmutableList2() };
        return view;
    }
}
