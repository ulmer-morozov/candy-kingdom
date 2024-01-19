using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class HomePage : PageFactory
{
    public const string ROUTE = "";

    public override LocalizedString Title => En("Home page!");

    public override string Route => ROUTE;

    public HomePage()
    {
        AddBones(new TextBone() { Title = En("Index page"), Text = En("Text on index page."), });
    }
}
