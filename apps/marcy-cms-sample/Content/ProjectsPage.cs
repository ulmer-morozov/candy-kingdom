using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class ProjectsPage : PageFactory
{
    public const string ROUTE = "projects";
    public override string Route => ROUTE;
    public override LocalizedString Title => En("Projects");

    public ProjectsPage()
    {
        AddBones(
            new PageListBone()
            {
                Title = En("Project-list"),
                DataRoute = ROUTE
            }
        );
    }
}
