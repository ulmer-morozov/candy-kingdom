namespace CandyKingdom.Marcy.Serialization;

public sealed class ProducerCache
{
    public Dictionary<string, Dictionary<string, FileSrc<ImageMeta>>> Images { get; set; } =
      new Dictionary<string, Dictionary<string, FileSrc<ImageMeta>>>();
}
