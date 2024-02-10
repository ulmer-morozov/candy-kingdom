using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;
using CandyKingdom.MarcyCms.Sample.Core;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class ProjectB : PageFactory<ProjectPageData>
{
    public override string Route { get; } = "b";
    public override LocalizedString Title { get; } = En("The Project \"B\"");
    public override OpenGraphData OpenGraph { get; } = new OpenGraphData
    {
        Title = En("OpenGraph Title of The Project \"B\""),
        Description = En("OpenGraph Description of The Project \"B\""),
        Image = En("")
    };
    public override ProjectPageData Data { get; } = new ProjectPageData
    {
        SpecialTitle = En("Project B Only Special Title!")
    };

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
