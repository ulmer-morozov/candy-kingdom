using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

using CandyKingdom.Marcy.ImageMin;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Storage;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.Serialization;

public sealed class ImageResizeConverter : JsonConverter<ImageM>, IUseFileCache
{
    public const string ImgSrcPrefix = "imgsrc";

    private readonly ImmutableList<ImageSetup> _setups;
    private readonly ImageConvertParameters _convertParameters;

    private readonly IImageUploader _imageUploader;

    public DirectoryInfo CacheDir { get; }

    public ImageResizeConverter(
      IImageManager imageManager,
      IEnumerable<ImageSetup> imageSetups,
      ImageConvertParameters convertParameters,
      DirectoryInfo cacheDir,
      IImageMin imageMin,
      IFileStorage fileStorage
    )
    {
        CacheDir = cacheDir;
        CacheDir.Create();

        _imageUploader = new ImageUploader
        (
            imageManager, fileStorage, imageMin,
            new ImageUploaderConfig
            {
                CacheDir = CacheDir
            }
        );

        _setups = imageSetups as ImmutableList<ImageSetup> ?? imageSetups.ToImmutableList();
        _convertParameters = convertParameters;
    }

    public override ImageM Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options
    )
    {
        throw new NotImplementedException("Cannot read");
    }

    public override void Write(Utf8JsonWriter writer, ImageM value, JsonSerializerOptions options)
    {
        WriteAsync(writer, value, options).Wait();
    }

    private async Task WriteAsync(
      Utf8JsonWriter writer,
      ImageM value,
      JsonSerializerOptions options,
      CancellationToken cancellationToken = default
    )
    {
        var imageSources = new List<ImageSource>();

        foreach (var imageSourceM in value.Sources)
        {
            var imageFile = new FileInfo(imageSourceM.Path);

            if (!imageFile.Exists)
            {
                throw new Exception($"Image file not found: {imageFile.FullName}");
            }

            using var imageStream = new DefferedStreamClone(
              () => new FileStream(imageFile.FullName, FileMode.Open)
            );

            var fileCacheInfo = await this.GetOrCreateFileCacheInfo(
              imageFile,
              imageStream,
              cancellationToken
            );

            Dictionary<ImageSetup, FileSrc<ImageMeta>> imageSrcDict = [];

            foreach (var setup in _setups)
            {
                var srcKeys = GetImgSrcKeys(fileCacheInfo, setup);

                var cachedFileSrc = await this.ReadFromCache<FileSrc<ImageMeta>>(
                  ImgSrcPrefix,
                  srcKeys,
                  cancellationToken
                );

                if (cachedFileSrc == null)
                {
                    continue;
                }

                imageSrcDict[setup] = cachedFileSrc;
            }

            var notCachedSetups = _setups.Except(imageSrcDict.Keys).ToImmutableList();

            if (!notCachedSetups.IsEmpty)
            {
                var fileSrcDict = await _imageUploader.ConvertAndStore(
                  imageStream.Value,
                  notCachedSetups,
                  _convertParameters,
                  cancellationToken
                );

                foreach (var pair in fileSrcDict)
                {
                    imageSrcDict[pair.Key] = pair.Value;

                    var srcKeys = GetImgSrcKeys(fileCacheInfo, pair.Key);

                    await this.StoreInCache(ImgSrcPrefix, srcKeys, pair.Value, cancellationToken);
                }
            }

            var imageSource = new ImageSource(imageSrcDict.Values) { MediaQuery = imageSourceM.MediaQuery ?? "" };

            imageSources.Add(imageSource);
        }

        var newImage = new Image(imageSources);

        var valueConverter = (JsonConverter<Image>)options.GetConverter(typeof(Image));
        valueConverter.Write(writer, newImage, options);
    }


    private ImmutableList<string> GetImgSrcKeys(FileCacheInfo fileCache, ImageSetup setup)
    {
        var srcKeys = ImmutableList.Create(
          fileCache.Hash,
          $"{_convertParameters}{setup}".CalcMd5AsBase62()
        );

        return srcKeys;
    }
}
