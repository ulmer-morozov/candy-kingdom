using CandyKingdom.Marcy.Skeleton;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public sealed record VimeoBone : Bone
{
    public required int VimeoId { get; init; }
    public required float Ratio { get; init; }
    public bool Autoplay { get; init; }
    public bool Muted { get; init; }
    public bool Loop { get; init; }

    public VimeoBone()
        : base("vimeo") { }
}
