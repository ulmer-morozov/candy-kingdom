namespace CandyKingdom.Marcy.ImageTools;

public interface IVideoManager
{
    public Task<TempVideoFile> AnalyseMp4(
      Stream sourceStream,
      CancellationToken cancellationToken = default
    );

    public Task Convert(
      Stream sourceStream,
      ICollection<VideoSetup> setups,
      VideoConvertParameters parameters,
      Func<VideoSetup, TempVideoFile, Task> action,
      CancellationToken cancellationToken = default
    );
}
