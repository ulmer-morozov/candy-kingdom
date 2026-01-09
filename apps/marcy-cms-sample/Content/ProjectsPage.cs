using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class ProjectsPage : PageFactory
{
    public override string Route { get; } = "projects";
    public override LocalizedString Title => En("Projects");
    public override OpenGraphData OpenGraph { get; } = new OpenGraphData
    {
        Title = En("OpenGraph Title of The Projects Page"),
        Description = En("OpenGraph Description of The Projects Page")
    };

    public ProjectsPage()
    {
        AddBones(
            new PageListBone()
            {
                Title = En("Project-list"),
                DataRoute = $"~/{Route}"
            }
        );
    }
}
