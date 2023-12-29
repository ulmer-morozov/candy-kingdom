namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinWebpOptions
{
    public uint Quality { get; }
    public bool Quiet { get; }

    public ImageMinWebpOptions(uint quality = 0, bool quiet = true)
    {
        if (quality > 100)
            throw new ArgumentException("Качество не может быть больше 100", nameof(quality));

        Quality = quality;
        Quiet = quiet;
    }
}
