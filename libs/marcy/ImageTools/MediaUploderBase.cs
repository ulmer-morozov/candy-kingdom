using CandyKingdom.Marcy.Storage;

namespace CandyKingdom.Marcy.ImageTools;

public abstract class MediaUploderBase
{
    protected readonly IFileStorage _fileStorage;

    protected abstract MediaUploaderBaseConfig Config { get; }

    protected MediaUploderBase(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }
}
