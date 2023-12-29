namespace CandyKingdom.Marcy.Storage;

public interface IFileStorage
{
    Task<StoredFile> Store(
        Stream stream,
        string name,
        string mimeType,
        CancellationToken cancellationToken = default
    );
}
