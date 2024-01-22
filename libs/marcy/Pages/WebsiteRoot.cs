namespace CandyKingdom.Marcy.Pages;

public sealed class WebsiteRoot : PageFactory
{
    public const string RootPrefix = "~";
    public override string Route => RootPrefix;

    public override LocalizedString Title { get; } = LocalizedString.Empty;
    public override OpenGraphData OpenGraph { get; } = OpenGraphData.Empty;
}
