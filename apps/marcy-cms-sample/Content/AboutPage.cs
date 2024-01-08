using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;
using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class AboutPage : PageFactory
{
    public const string ROUTE = "about";
    public override LocalizedString Title => En("About");

    public override string Route => ROUTE;

    public AboutPage()
    {
        AddBones(new TextBone() { Title = En("About page"), Text = En("Some text example") });
    }
}
