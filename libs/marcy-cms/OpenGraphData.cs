using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms;

public sealed record OpenGraphData
{
    public LocalizedString Title { get; init; } = LocalizedString.Empty;
    public LocalizedString Description { get; init; } = LocalizedString.Empty;
    public LocalizedString Image { get; init; } = LocalizedString.Empty;
}
