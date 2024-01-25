using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy.Pages;
public abstract class ViewFactory : SkeletonFactory
{
    public abstract string Code { get; }

    public View Create()
    {
        var view = new View { Code = Code, Bones = Bones.ToImmutableList2() };
        return view;
    }
}
