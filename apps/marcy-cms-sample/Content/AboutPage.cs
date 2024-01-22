using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class AboutPage : PageFactory
{
    public override string Route { get; } = "about";
    public override LocalizedString Title { get; } = En("About");
    public override OpenGraphData OpenGraph { get; } = new OpenGraphData
    {
        Title = En("OpenGraph Title of The About Page"),
        Description = En("OpenGraph Description of The About Page"),
        Image = En("")
    };

    public AboutPage()
    {
        AddBones(new TextBone()
        {
            Title = En("About page"),
            Text = En("Some text example")
        });
    }
}
