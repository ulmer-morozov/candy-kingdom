using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class ProjectB : PageFactory
{
    public const string ROUTE = "b";

    public override LocalizedString Title { get; } = En("The Project \"B\"");

    public override string Route { get; } = ROUTE;

    public ProjectB()
    {
        AddBones(
            new TextBone()
            {
                Title = En("The Project \"B\""),
                Text = En("The Buck")
            },
            new VimeoBone() { VimeoId = 430695671, Ratio = 1.77777f }
        );
    }
}
