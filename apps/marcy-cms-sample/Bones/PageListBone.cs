using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Skeleton;
using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public sealed record PageListBone : Bone, IHaveDataRouteWithData<string>
{
    public LocalizedString Title { get; init; } = Empty;
    public string DataRoute { get; init; } = "";
    public string Data { get; init; } = "";

    public PageListBone()
        : base("page-list") { }
}
