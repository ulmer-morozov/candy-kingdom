using System.Collections.Immutable;

namespace CandyKingdom.Marcy.ImageTools;

public interface IVideoUploader
{
    public Task<ImmutableDictionary<VideoSetup, FileSrc<VideoMeta>>> ConvertAndStore(
       MemoryStream value,
       ImmutableList<VideoSetup> setups,
       VideoConvertParameters convertParameters,
       Func<VideoSetup, TempVideoFile, FileSrc<VideoMeta>, Task>? action = null,
       CancellationToken cancellationToken = default
     );
}
