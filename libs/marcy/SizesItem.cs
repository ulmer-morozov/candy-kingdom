namespace CandyKingdom.Marcy;

public sealed record SizesItem
{
    public required int Width { get; init; }
    public SizesWidthUnit Unit { get; init; } = SizesWidthUnit.Px;
    public string MediaQuery { get; init; } = "";
}
