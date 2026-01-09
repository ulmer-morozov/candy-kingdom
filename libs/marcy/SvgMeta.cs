namespace CandyKingdom.Marcy;

public sealed record SvgMeta : PixMeta
{
    public decimal PreciseWidth { get; init; }
    public decimal PreciseHeight { get; init; }
}
