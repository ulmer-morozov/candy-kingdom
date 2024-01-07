namespace CandyKingdom.Marcy.ImageTools;

public interface IImageManager
{
    public bool HasTransparency(Stream imageStream);

    public Task Convert(
      Stream sourceStream,
      ICollection<ImageSetup> setups,
      Func<ImageSetup, InMemoryImage, Task> action,
      CancellationToken cancellationToken = default
    );
}
