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
    public const string VideoFilePrefix = "videofile";

    private readonly ImmutableList<VideoSetup> _setups;
    private readonly IVideoUploader _videoUploader;


    public DirectoryInfo CacheDir { get; }
    private readonly VideoConvertParameters _videoConvertParameters;

    public VideoResizeConverter(
      IVideoManager videoManager,
      ImmutableList<VideoSetup> setups,
      DirectoryInfo cacheDir,
      IFileStorage fileStorage,
      VideoConvertParameters videoConvertParameters
    )
    {
        CacheDir = cacheDir;
        CacheDir.Create();

        _setups = setups;
        _videoConvertParameters = videoConvertParameters;

        _videoUploader = new VideoUploader
        (
            fileStorage, videoManager, new VideoUploaderConfig
            {
                CacheDir = CacheDir
            }
        );
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
            {
                throw new Exception($"Image file not found: {videoFile.FullName}");
            }

            using var videoStream = new DefferedStreamClone(
              () => new FileStream(videoFile.FullName, FileMode.Open)
            );

            var fileCacheInfo = await this.GetOrCreateFileCacheInfo(
              videoFile,
              videoStream,
              cancellationToken
            );

            Dictionary<VideoSetup, FileSrc<VideoMeta>> srcDict = [];

            foreach (var setup in _setups)
            {
                var srcKeys = GetVideoSrcKeys(fileCacheInfo, setup);

                var cachedFileSrc = await this.ReadJsonFromCache<FileSrc<VideoMeta>>(
                  VideoSrcPrefix,
                  srcKeys,
                  cancellationToken
                );

                if (cachedFileSrc == null)
                {
                    continue;
                }

                var cachedFile = this.ReadFileFromCache(
                  VideoSrcPrefix,
                  srcKeys,
                  setup.Format.Extension
                );

                if (cachedFile == null)
                {
                    continue;
                }

                srcDict[setup] = cachedFileSrc;
            }

            var notCachedSetups = _setups.Except(srcDict.Keys).ToImmutableList();

            if (!notCachedSetups.IsEmpty)
            {

                async Task SetCache(VideoSetup setup, TempVideoFile videoFile, FileSrc<VideoMeta> fileSrc)
                {
                    var srcKeys = GetVideoSrcKeys(fileCacheInfo, setup);

                    await this.StoreJsonInCache(VideoSrcPrefix, srcKeys, fileSrc, cancellationToken);
                    await this.StoreFileInCache(VideoFilePrefix, srcKeys, videoFile.File, cancellationToken);
                }

                var fileSrcDict = await _videoUploader.ConvertAndStore(
                  videoStream.Value,
                  notCachedSetups,
                  _videoConvertParameters,
                  SetCache,
                  cancellationToken
                );

                foreach (var pair in fileSrcDict)
                {
                    srcDict[pair.Key] = pair.Value;
                }
            }

            var videoSource = new VideoSource(srcDict.Values) { MediaQuery = sourceM.MediaQuery ?? "" };

            sources.Add(videoSource);
        }

        var newVideo = new Video(sources);

        var valueConverter = (JsonConverter<Video>)options.GetConverter(typeof(Video));
        valueConverter.Write(writer, newVideo, options);
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
