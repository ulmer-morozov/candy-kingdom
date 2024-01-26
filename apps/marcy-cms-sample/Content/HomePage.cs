using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class HomePage : PageFactory
{
    public override string Route { get; } = "";
    public override LocalizedString Title { get; } = En("Home");
    public override OpenGraphData OpenGraph { get; } = new OpenGraphData
    {
        Title = En("OpenGraph Title of The Home Page"),
        Description = En("OpenGraph Description of The Home Page"),
        Image = En("")
    };

    public HomePage()
    {
        AddBones(new TextBone()
        {
            Title = En("Index page"),
            Text = En("Text on index page.")
        });
    }
}
