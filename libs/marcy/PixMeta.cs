namespace CandyKingdom.Marcy;

public record PixMeta : FileMeta
{
    public required int Width { get; init; }
    public required int Height { get; init; }
    public float Ratio => Height == 0 ? 0 : float.Round((float)Width / Height, 5);
}
