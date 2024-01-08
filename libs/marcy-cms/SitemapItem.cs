using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.MarcyCms;

public sealed record SitemapItem
{
    public string Loc { get; init; } = "";
    public string ChangeFreq { get; init; } = "";
    public string Priority { get; init; } = "";

    public ImmutableList2<SiteMapItemAlternate> Alternates { get; init; } = ImmutableList2<SiteMapItemAlternate>.Empty;
}
