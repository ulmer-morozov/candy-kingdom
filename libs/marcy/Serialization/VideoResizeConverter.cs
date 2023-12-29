using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Storage;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.Serialization;

public sealed class VideoResizeConverter : JsonConverter<VideoM>, IUseFileCache
{
  public const string VideoSrcPrefix = "videosrc";

  private readonly IVideoManager _videoManager;
  private readonly IFileStorage _fileStorage;
  private readonly ImmutableList<VideoSetup> _setups;
  private readonly DirectoryInfo _cacheDir;

  public DirectoryInfo CacheDir => _cacheDir;
  private readonly VideoConvertParameters _videoConvertParameters;

  public VideoResizeConverter(
    IVideoManager videoManager,
    ImmutableList<VideoSetup> setups,
    DirectoryInfo cacheDir,
    IFileStorage fileStorage,
    VideoConvertParameters videoConvertParameters
  )
  {
    _videoManager = videoManager;
    _setups = setups;
    _cacheDir = cacheDir;
    _fileStorage = fileStorage;
    _videoConvertParameters = videoConvertParameters;
  }

  public override VideoM Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    throw new NotImplementedException("Cannot read");
  }

  public override void Write(Utf8JsonWriter writer, VideoM value, JsonSerializerOptions options)
  {
    WriteAsync(writer, value, options).Wait();
  }

  private async Task WriteAsync(
    Utf8JsonWriter writer,
    VideoM value,
    JsonSerializerOptions options,
    CancellationToken cancellationToken = default
  )
  {
    var sources = new List<VideoSource>();

    foreach (var sourceM in value.Sources)
    {
      var videoFile = new FileInfo(sourceM.Path);

      if (!videoFile.Exists)
        throw new Exception($"Image file not found: {videoFile.FullName}");

      using var videoStream = new DefferedStreamClone(
        () => new FileStream(videoFile.FullName, FileMode.Open)
      );

      var fileCacheInfo = await this.GetOrCreateFileCacheInfo(
        videoFile,
        videoStream,
        cancellationToken
      );

      Dictionary<VideoSetup, FileSrc<VideoMeta>> srcDict = new();

      foreach (var setup in _setups)
      {
        var srcKeys = GetVideoSrcKeys(fileCacheInfo, setup);

        var cachedFileSrc = await this.ReadFromCache<FileSrc<VideoMeta>>(
          VideoSrcPrefix,
          srcKeys,
          cancellationToken
        );

        if (cachedFileSrc == null)
          continue;

        srcDict[setup] = cachedFileSrc;
      }

      var notCachedSetups = _setups.Except(srcDict.Keys).ToImmutableList();

      if (notCachedSetups.Any())
      {
        var fileSrcDict = await ConvertAndStore(
          fileCacheInfo,
          videoStream.Value,
          notCachedSetups,
          cancellationToken
        );

        foreach (var pair in fileSrcDict)
        {
          srcDict[pair.Key] = pair.Value;
        }
      }

      var orderedSrcs = srcDict
        .Values.OrderByDescending(x => x.Meta.Width)
        .ThenByDescending(x => x.MimeType.Contains("web")); // webm first

      var videoSource = new VideoSource(orderedSrcs) { MediaQuery = sourceM.MediaQuery ?? "" };

      sources.Add(videoSource);
    }

    var newVideo = new Video(sources);

    var valueConverter = (JsonConverter<Video>)options.GetConverter(typeof(Video));
    valueConverter.Write(writer, newVideo, options);
  }

  private async Task<ImmutableDictionary<VideoSetup, FileSrc<VideoMeta>>> ConvertAndStore(
    FileCacheInfo fileCacheInfo,
    MemoryStream value,
    ImmutableList<VideoSetup> setups,
    CancellationToken cancellationToken
  )
  {
    var dict = new Dictionary<VideoSetup, FileSrc<VideoMeta>>();

    async Task OnVideo(VideoSetup setup, TempVideoFile videoFile)
    {
      StoredFile storedFile;

      using (
        var fileStream = new FileStream(videoFile.File.FullName, FileMode.Open, FileAccess.Read)
      )
      {
        var hash = await fileStream.CalcMd5AsBase62Async(cancellationToken);

        var fileName =
          $"{videoFile.Meta.Width}x{videoFile.Meta.Height}_{hash}{videoFile.Format.Extension}";

        Console.WriteLine($"On video store: {fileName}");

        fileStream.Seek(0, SeekOrigin.Begin);

        storedFile = await _fileStorage.Store(
          stream: fileStream,
          name: fileName,
          mimeType: videoFile.Format.MimeType,
          cancellationToken
        );
      }

      Console.WriteLine($"On video stored: {storedFile.Url}");

      var fileSrc = new FileSrc<VideoMeta>
      {
        Url = storedFile.Url,
        Meta = videoFile.Meta,
        MimeType = videoFile.Format.MimeType
      };

      dict[setup] = fileSrc;

      var srcKeys = GetVideoSrcKeys(fileCacheInfo, setup);

      await this.StoreInCache(VideoSrcPrefix, srcKeys, fileSrc, cancellationToken);

      videoFile.Dispose();
    }

    await _videoManager.Convert(value, setups, _videoConvertParameters, OnVideo, cancellationToken);

    return dict.ToImmutableDictionary();
  }

  private ImmutableList<string> GetVideoSrcKeys(FileCacheInfo fileCache, VideoSetup setup)
  {
    var srcKeys = ImmutableList.Create(
      fileCache.Hash,
      $"{setup}{_videoConvertParameters}".CalcMd5AsBase62()
    );
    return srcKeys;
  }
}
