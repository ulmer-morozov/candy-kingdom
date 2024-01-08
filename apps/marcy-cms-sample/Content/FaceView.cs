using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;
using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class FaceView : ViewFactory
{
    public override string Id { get; } = "face";

    public FaceView()
    {
        AddBones(
            new PageListBone()
            {
                Title = En(""),
                DataRoute = "~",
                Style = "main-nav"
            }
        );
    }
}
