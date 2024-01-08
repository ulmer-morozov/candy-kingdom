using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Skeleton;
using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public sealed record TextBone : Bone
{
    public LocalizedString Title { get; init; } = Empty;
    public LocalizedString Text { get; init; } = Empty;

    public TextBone()
        : base("text") { }
}
