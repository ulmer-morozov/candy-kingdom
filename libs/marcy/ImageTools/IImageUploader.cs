using System.Collections.Immutable;

namespace CandyKingdom.Marcy.ImageTools;

public interface IImageUploader
{
    public Task<ImmutableDictionary<ImageSetup, FileSrc<ImageMeta>>> ConvertAndStore(
          MemoryStream imageStream,
          ImmutableList<ImageSetup> setups,
          ImageConvertParameters convertParameters,
          CancellationToken cancellationToken
    );

    public Task<FileSrc<ImageMeta>> ConvertAndStore(
        MemoryStream imageStream,
        ImageSetup setup,
        ImageConvertParameters convertParameters,
        CancellationToken cancellationToken
  );
}
