using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

using CandyKingdom.Marcy.ImageMin;
using CandyKingdom.Marcy.Storage;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageTools;

public sealed class ImageUploader : MediaUploderBase, IImageUploader
{
    private readonly IImageManager _imageManager;
    private readonly IImageMin _imageMin;

    protected override ImageUploaderConfig Config { get; }

    public ImageUploader(IImageManager imageManager, IFileStorage fileStorage, IImageMin imageMin, ImageUploaderConfig config)
        : base(fileStorage)
    {
        _imageManager = imageManager;
        _imageMin = imageMin;

        Config = config;
    }

    public async Task<ImmutableDictionary<ImageSetup, FileSrc<ImageMeta>>> ConvertAndStore(
          MemoryStream imageStream,
          ImmutableList<ImageSetup> setups,
          ImageConvertParameters convertParameters,
          Func<ImageSetup, FileInfo, FileSrc<ImageMeta>, string, Task>? action = null,
          CancellationToken cancellationToken = default
        )
    {
        var filePaths = new List<(ImageSetup, FileInfo, ImageMeta)>();

        async Task OnImage(ImageSetup setup, InMemoryImage inMemoryImage)
        {
            var fileName = $"tmp_{Guid.NewGuid()}{inMemoryImage.Format.Extension}";
            var filePath = Path.Combine(Config.CacheDir.FullName, fileName);

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
              Trace.WriteLine($"ACTION BLOCK {x.file.Name}. {x.file.Length}");

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

              Trace.WriteLine($"TRANSFORM BLOCK store: {fileName}");

              StoredFile storedFile;

              using (var fileStream = new FileStream(x.file.FullName, FileMode.Open, FileAccess.Read))
              {
                  storedFile = await _fileStorage.Store(
                stream: fileStream,
                name: fileName,
                mimeType: x.setup.Format.MimeType,
                cancellationToken
              );
              }

              Trace.WriteLine($"TRANSFORM BLOCK stored: {storedFile.Url}");

              var fileSrc = new FileSrc<ImageMeta>
              {
                  Url = storedFile.Url,
                  Meta = x.meta,
                  MimeType = x.setup.Format.MimeType,
              };

              if (action != null)
              {
                  await action(x.setup, x.file, fileSrc, x.hash);
              }

              x.file.Delete();

              return (x.setup, fileSrc);
          },
          dataFlowBlockOptions
        );

        var finalBlock = new BufferBlock<(ImageSetup setup, FileSrc<ImageMeta> fileSrc)>();

        bufferBlock.LinkTo(printBlock, dataFlowLinkOptions);

        if (convertParameters.Minify)
        {
            var minifyBlock = new TransformBlock<
              (ImageSetup setup, FileInfo file, ImageMeta meta),
              (ImageSetup setup, FileInfo file, ImageMeta, string hash)
            >(
              async x =>
              {
                  using var imageFileStream = new MemoryStream();

                  using (var fileStream = new FileStream(x.file.FullName, FileMode.Open, FileAccess.Read))
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
                  var filePath = Path.Combine(Config.CacheDir.FullName, fileName);

                  Trace.WriteLine(
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

                  using (var fileStream = new FileStream(x.file.FullName, FileMode.Open, FileAccess.Read))
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

        Trace.WriteLine($"ELLAPSED {stopTime.Elapsed}");

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

    public async Task<FileSrc<ImageMeta>> ConvertAndStore(MemoryStream imageStream, ImageSetup setup, ImageConvertParameters convertParameters, CancellationToken cancellationToken)
    {
        var setups = ImmutableList.Create(setup);

        var images = await ConvertAndStore(imageStream, setups, convertParameters, null, cancellationToken);

        return images[setup];
    }
}
