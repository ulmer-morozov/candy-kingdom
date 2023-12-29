using System.Collections.Immutable;
using System.Text.Json;
using CandyKingdom.Marcy.ImageMin;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Storage;

namespace CandyKingdom.Marcy.Serialization;

public sealed class SmartJsonProducer : ISmartJsonProducer
{
  private readonly IImageManager _imageManager;
  private readonly IVideoManager _videoManager;
  private readonly IImageMin _imageMin;
  private readonly IFileStorage _fileStorage;

  public SmartJsonProducer(
    IImageManager imageManager,
    IVideoManager videoManager,
    IImageMin imageMin,
    IFileStorage fileStorage
  )
  {
    _imageManager = imageManager;
    _videoManager = videoManager;
    _imageMin = imageMin;
    _fileStorage = fileStorage;
  }

  public string SerializeAndInject<T>(
    T obj,
    JsonSerializerOptions jsonSerializerOptions,
    ImageConvertParameters convertParameters,
    ImmutableList<ImageSetup> imageSetups,
    ImmutableList<VideoSetup> videoSetups
  )
  {
    var cacheDir = new DirectoryInfo(Path.Combine("marcy-cache"));

    var imageResizeConverter = new ImageResizeConverter(
      _imageManager,
      imageSetups,
      convertParameters,
      cacheDir,
      imageMin: _imageMin,
      fileStorage: _fileStorage
    );

    var videoConverter = new VideoResizeConverter(
      _videoManager,
      videoSetups,
      cacheDir,
      _fileStorage,
      new VideoConvertParameters { RemoveAudio = true }
    );

    jsonSerializerOptions.Converters.Add(imageResizeConverter);
    jsonSerializerOptions.Converters.Add(videoConverter);

    var imageJson = JsonSerializer.Serialize(obj, jsonSerializerOptions);

    return imageJson;
  }
}
