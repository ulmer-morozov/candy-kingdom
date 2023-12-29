using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Utilities;
using CandyKingdom.Marcy.ImageMin;
using CandyKingdom.Marcy.Storage;

namespace CandyKingdom.Marcy.Serialization;

public sealed class ImageResizeConverter : JsonConverter<ImageM>, IUseFileCache
{
    public const string ImgSrcPrefix = "imgsrc";

    private readonly IImageManager _imageManager;
    private readonly IImageMin _imageMin;
    private readonly ImmutableList<ImageSetup> _setups;
    private readonly ImageConvertParameters _convertParameters;
    private readonly DirectoryInfo _cacheDir;
    private readonly IFileStorage _fileStorage;

    public DirectoryInfo CacheDir => _cacheDir;

    public ImageResizeConverter(
        IImageManager imageManager,
        IEnumerable<ImageSetup> imageSetups,
        ImageConvertParameters convertParameters,
        DirectoryInfo cacheDir,
        IImageMin imageMin,
        IFileStorage fileStorage
    )
    {
        _imageManager = imageManager;
        _setups = imageSetups as ImmutableList<ImageSetup> ?? imageSetups.ToImmutableList();
        _convertParameters = convertParameters;
        _cacheDir = cacheDir;

        _cacheDir.Create();
        _imageMin = imageMin;
        _fileStorage = fileStorage;
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
                throw new Exception($"Image file not found: {imageFile.FullName}");

            using var imageStream = new DefferedStreamClone(
                () => new FileStream(imageFile.FullName, FileMode.Open)
            );

            var fileCacheInfo = await this.GetOrCreateFileCacheInfo(
                imageFile,
                imageStream,
                cancellationToken
            );

            Dictionary<ImageSetup, FileSrc<ImageMeta>> imageSrcDict = new();

            foreach (var setup in _setups)
            {
                var srcKeys = GetImgSrcKeys(fileCacheInfo, setup);

                var cachedFileSrc = await this.ReadFromCache<FileSrc<ImageMeta>>(
                    ImgSrcPrefix,
                    srcKeys,
                    cancellationToken
                );

                if (cachedFileSrc == null)
                    continue;

                imageSrcDict[setup] = cachedFileSrc;
            }

            var notCachedSetups = _setups.Except(imageSrcDict.Keys).ToImmutableList();

            if (notCachedSetups.Any())
            {
                var fileSrcDict = await ConvertAndStore(
                    imageStream.Value,
                    notCachedSetups,
                    cancellationToken
                );

                foreach (var pair in fileSrcDict)
                {
                    imageSrcDict[pair.Key] = pair.Value;

                    var srcKeys = GetImgSrcKeys(fileCacheInfo, pair.Key);

                    await this.StoreInCache(ImgSrcPrefix, srcKeys, pair.Value, cancellationToken);
                }
            }

            var orderedSrcs = imageSrcDict.Values
                .OrderByDescending(x => x.Meta.Width)
                .ThenByDescending(x => x.MimeType.Contains("web")); // webp first)

            var imageSource = new ImageSource(orderedSrcs)
            {
                MediaQuery = imageSourceM.MediaQuery ?? ""
            };

            imageSources.Add(imageSource);
        }

        var newImage = new Image(imageSources);

        var valueConverter = (JsonConverter<Image>)options.GetConverter(typeof(Image));
        valueConverter.Write(writer, newImage, options);
    }

    private async Task<ImmutableDictionary<ImageSetup, FileSrc<ImageMeta>>> ConvertAndStore(
        MemoryStream imageStream,
        ImmutableList<ImageSetup> setups,
        CancellationToken cancellationToken
    )
    {
        var filePaths = new List<(ImageSetup, FileInfo, ImageMeta)>();

        async Task OnImage(ImageSetup setup, InMemoryImage inMemoryImage)
        {
            var fileName = $"tmp_{Guid.NewGuid()}{inMemoryImage.Format.Extension}";
            var filePath = Path.Combine(CacheDir.FullName, fileName);

            using var fileStream = new FileStream(filePath, FileMode.CreateNew);
            await inMemoryImage.Stream.CopyToAsync(fileStream, cancellationToken);

            filePaths.Add((setup, new FileInfo(filePath), inMemoryImage.Meta));

            inMemoryImage.Dispose();
        }

        await _imageManager.Convert(imageStream, setups, OnImage, cancellationToken);

        var bufferBlock = new BufferBlock<(ImageSetup, FileInfo, ImageMeta)>();

        var dataFlowBlockOptions = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        var dataFlowLinkOptions = new DataflowLinkOptions { PropagateCompletion = true };

        var stopTime = new Stopwatch();
        stopTime.Start();

        var printBlock = new TransformBlock<
            (ImageSetup setup, FileInfo file, ImageMeta),
            (ImageSetup setup, FileInfo file, ImageMeta)
        >(
            x =>
            {
                Console.WriteLine($"ACTION BLOCK {x.file.Name}. {x.file.Length}");

                return x;
            },
            dataFlowBlockOptions
        );

        var storeBlock = new TransformBlock<
            (ImageSetup setup, FileInfo file, ImageMeta meta, string hash),
            (ImageSetup setup, FileSrc<ImageMeta> fileSrc)
        >(
            async x =>
            {
                var fileName = $"{x.meta.Width}x{x.meta.Height}_{x.hash}{x.setup.Format.Extension}";

                Console.WriteLine($"TRANSFORM BLOCK store: {fileName}");

                StoredFile storedFile;

                using (
                    var fileStream = new FileStream(x.file.FullName, FileMode.Open, FileAccess.Read)
                )
                {
                    storedFile = await _fileStorage.Store(
                        stream: fileStream,
                        name: fileName,
                        mimeType: x.setup.Format.MimeType,
                        cancellationToken
                    );
                }

                Console.WriteLine($"TRANSFORM BLOCK stored: {storedFile.Url}");

                var fileSrc = new FileSrc<ImageMeta>
                {
                    Url = storedFile.Url,
                    Meta = x.meta,
                    MimeType = x.setup.Format.MimeType,
                };

                x.file.Delete();

                return (x.setup, fileSrc);
            },
            dataFlowBlockOptions
        );

        var finalBlock = new BufferBlock<(ImageSetup setup, FileSrc<ImageMeta> fileSrc)>();

        bufferBlock.LinkTo(printBlock, dataFlowLinkOptions);

        if (_convertParameters.Minify)
        {
            var minifyBlock = new TransformBlock<
                (ImageSetup setup, FileInfo file, ImageMeta meta),
                (ImageSetup setup, FileInfo file, ImageMeta, string hash)
            >(
                async x =>
                {
                    using var imageFileStream = new MemoryStream();

                    using (
                        var fileStream = new FileStream(
                            x.file.FullName,
                            FileMode.Open,
                            FileAccess.Read
                        )
                    )
                    {
                        await fileStream.CopyToAsync(imageFileStream);
                    }

                    var hash = await imageFileStream.CalcMd5AsBase62Async();

                    var minifiedImageMs = await _imageMin.Minify(
                        imageFileStream,
                        x.setup.Format.Extension,
                        cancellationToken
                    );

                    minifiedImageMs.Seek(0, SeekOrigin.Begin);

                    var fileName = $"tmp_{hash}_{Guid.NewGuid()}";
                    var filePath = Path.Combine(CacheDir.FullName, fileName);

                    Console.WriteLine(
                        $"TRANSFORM BLOCK minified: ({imageFileStream.Length - minifiedImageMs.Length}) {fileName}"
                    );


                    using var minifiedFileStream = new FileStream(filePath, FileMode.CreateNew);
                    {
                        await minifiedImageMs.CopyToAsync(minifiedFileStream, cancellationToken);
                    }

                    x.file.Delete();
                    minifiedImageMs.Dispose();

                    return (x.setup, new FileInfo(filePath), x.meta, hash);
                },
                dataFlowBlockOptions
            );

            printBlock.LinkTo(minifyBlock, dataFlowLinkOptions);
            minifyBlock.LinkTo(storeBlock, dataFlowLinkOptions);
        }
        else
        {
            var calcHashBlock = new TransformBlock<
                (ImageSetup setup, FileInfo file, ImageMeta meta),
                (ImageSetup setup, FileInfo file, ImageMeta, string hash)
            >(
                async x =>
                {
                    string hash;

                    using (
                        var fileStream = new FileStream(
                            x.file.FullName,
                            FileMode.Open,
                            FileAccess.Read
                        )
                    )
                    {
                        hash = await fileStream.CalcMd5AsBase62Async();
                    }

                    return (x.setup, x.file, x.meta, hash);
                },
                dataFlowBlockOptions
            );

            printBlock.LinkTo(calcHashBlock, dataFlowLinkOptions);
            calcHashBlock.LinkTo(storeBlock, dataFlowLinkOptions);
        }

        storeBlock.LinkTo(finalBlock, dataFlowLinkOptions);

        filePaths.ForEach(x => bufferBlock.Post(x));
        bufferBlock.Complete();

        await storeBlock.Completion;
        stopTime.Stop();

        Console.WriteLine($"ELLAPSED {stopTime.Elapsed}");

        var dict = new Dictionary<ImageSetup, FileSrc<ImageMeta>>();

        while (finalBlock.Count > 0)
        {
            var (setup, fileSrc) = await finalBlock.ReceiveAsync(cancellationToken);
            dict[setup] = fileSrc;
        }

        // var all = storeBlock
        //     .ReceiveAllAsync(cancellationToken)
        //     .ToBlockingEnumerable(cancellationToken)
        //     .ToImmutableDictionary(x => x.setup, x => x.fileSrc);

        return dict.ToImmutableDictionary();
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
