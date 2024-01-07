namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinJpegtranOptions(bool progressive = false)
{
    public bool Progressive { get; } = progressive;
}
