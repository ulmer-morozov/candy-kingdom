using System.Collections.Immutable;

namespace CandyKingdom.Marcy.ImageTools;

public interface IImageUploader
{
    public Task<ImmutableDictionary<ImageSetup, FileSrc<ImageMeta>>> ConvertAndStore(
          MemoryStream imageStream,
          ImmutableList<ImageSetup> setups,
          ImageConvertParameters convertParameters,
          Func<ImageSetup, FileInfo, FileSrc<ImageMeta>, string, Task>? action = null,
          CancellationToken cancellationToken = default
    );

    public Task<FileSrc<ImageMeta>> ConvertAndStore(
        MemoryStream imageStream,
        ImageSetup setup,
        ImageConvertParameters convertParameters,
        CancellationToken cancellationToken
  );
}
