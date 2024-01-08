using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public static class MediaBoneStyle
{
    public const string MobileDisplay = "mobile-display";
}

public sealed record MediaBone : Bone
{
    public required PixMedia Media { get; init; }
    public LocalizedString Title { get; init; } = LocalizedString.Empty;
    public LocalizedString Text { get; init; } = LocalizedString.Empty;
    public LocalizedString Alt { get; init; } = LocalizedString.Empty;
    public LocalizedString Link { get; init; } = LocalizedString.Empty;

    public MediaBone()
        : base("media") { }
}
