using System.Text.Json.Serialization;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public sealed record PageListBone : Bone, IHaveDataRouteWithData<ImmutableList2<Page>>
{
    public const string BoneType = "page-list";
    public LocalizedString Title { get; init; } = Empty;
    public string DataRoute { get; init; } = "";

    [JsonIgnore]
    public ImmutableList2<Page> Data { get; init; } = ImmutableList2<Page>.Empty;

    public PageListBone()
        : base(BoneType) { }
}
