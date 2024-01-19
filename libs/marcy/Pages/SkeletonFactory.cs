using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.Marcy.Pages;

public abstract class SkeletonFactory
{
    public List<Bone> Bones { get; } = [];

    protected void AddBones(params Bone[] bones) => Bones.AddRange(bones);
}
