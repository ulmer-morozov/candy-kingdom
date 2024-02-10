using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy.Serialization;

[JsonSerializable(typeof(PixMedia))]
[JsonSerializable(typeof(Video))]
[JsonSerializable(typeof(Image))]
[JsonSerializable(typeof(VideoSource))]
[JsonSerializable(typeof(FileSrc<ImageMeta>))]
[JsonSerializable(typeof(FileSrc<VideoMeta>))]
public partial class MarcyJsonContext : JsonSerializerContext
{
}
