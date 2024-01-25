using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Data;

public sealed class ViewDb
{
    public Guid Id { get; private set; }
    public string Code { get; private set; }

    public ImmutableList2<Bone> Bones { get; private set; }

    public ViewDb(Guid id, string code, IEnumerable<Bone> bones)
    {
        Id = id;
        Code = code;

        Bones = bones as ImmutableList2<Bone>
                            ?? bones?.ToImmutableList2()
                            ?? ImmutableList2<Bone>.Empty;
    }

    private ViewDb()
    {
        Code = null!;
        Bones = null!;
    }
}
