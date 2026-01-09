using System.Collections.Immutable;

namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMin : IImageMin
{
    private readonly ImmutableList<ImageMinVendor> _vendors;

    public ImageMin(params ImageMinVendor[] vendors)
      : this(vendors as IEnumerable<ImageMinVendor>) { }

    public ImageMin(IEnumerable<ImageMinVendor> vendors)
    {
        _vendors = vendors?.ToImmutableList() ?? [];
    }

    public async Task<MemoryStream> Minify(
      MemoryStream sourceStream,
      string format,
      CancellationToken cancellationToken = default
    )
    {
        var tempFilePath = await StoreTempFile(sourceStream, format, cancellationToken);

        var bestCompressedStream = new MemoryStream();
        await sourceStream.CopyToAsync(bestCompressedStream, cancellationToken);

        sourceStream.Seek(0, SeekOrigin.Begin);
        bestCompressedStream.Seek(0, SeekOrigin.Begin);

        var initialSize = bestCompressedStream.Length;
        var bestFileSize = initialSize;

        foreach (var vendor in _vendors)
        {
            if (!vendor.CanBeApplyed(format))
            {
                continue;
            }

            var minifiedImageStream = await vendor.Minify(tempFilePath, cancellationToken);

            if (minifiedImageStream.Length >= bestFileSize || minifiedImageStream.Length == 0)
            {
                Console.WriteLine(
                  $"!{vendor.Name} minified worse than best result. delta: {bestFileSize - minifiedImageStream.Length}. Size: {minifiedImageStream.Length}"
                );
                await minifiedImageStream.DisposeAsync();
                continue;
            }

            Console.WriteLine(
              $"{vendor.Name} minified perfectly! delta: {initialSize - minifiedImageStream.Length}. Size: {minifiedImageStream.Length}"
            );

            await bestCompressedStream.DisposeAsync();

            bestCompressedStream = minifiedImageStream;
            bestFileSize = minifiedImageStream.Length;
        }

        File.Delete(tempFilePath);

        return bestCompressedStream;
    }

    private static async Task<string> StoreTempFile(
      MemoryStream sourceStream,
      string format,
      CancellationToken cancellationToken = default
    )
    {
        sourceStream.Seek(0, SeekOrigin.Begin);

        var tempDirPath = Path.GetTempPath();
        var guid = Guid.NewGuid().ToString().Replace("-", "");

        var tempFileName = $"minify-{guid}{format}";
        var tempFilePath = Path.Combine(tempDirPath, tempFileName);

        await using (var fileStream = new FileStream(tempFilePath, FileMode.CreateNew))
        {
            await sourceStream.CopyToAsync(fileStream, cancellationToken);
        }

        sourceStream.Seek(0, SeekOrigin.Begin);
        return tempFilePath;
    }
}
