namespace CandyKingdom.Marcy.Pages;

public sealed class WebsiteRoot : PageFactory
{
    public const string ROUTE = "~";

    public override LocalizedString Title { get; } = LocalizedString.Empty;

    public override string Route => ROUTE;
}
