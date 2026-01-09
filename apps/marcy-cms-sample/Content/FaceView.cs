using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class FaceView : ViewFactory
{
    public override string Code { get; } = "face";

    public FaceView()
    {
        AddBones(
            new PageListBone()
            {
                Title = Empty,
                DataRoute = "~",
                Style = PageListBoneStyle.MainNav
            }
        );
    }
}
