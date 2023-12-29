namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinGuetzliOptions
{
  public uint Quality { get; }
  public uint MemoryLimit { get; }

  public ImageMinGuetzliOptions(uint quality = 0, uint memoryLimit = 0)
  {
    if (quality > 100)
      throw new ArgumentException("Качество не может быть больше 100", nameof(quality));

    Quality = quality;
    MemoryLimit = memoryLimit;
  }
}
