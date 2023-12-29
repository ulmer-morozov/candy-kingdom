namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinMozJpegOptions
{
    public uint Quality { get; }
    public uint MaxMemory { get; }

    public ImageMinMozJpegOptions(uint quality = 0, uint maxMemory = 0)
    {
        if (quality > 100)
            throw new ArgumentException("Качество не может быть больше 100", nameof(quality));

        Quality = quality;
        MaxMemory = maxMemory;
    }
}
