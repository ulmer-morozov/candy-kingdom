namespace CandyKingdom.Marcy.ImageMin;

public interface IImageMin
{
    public Task<MemoryStream> Minify(
        MemoryStream sourceStream,
        string format,
        CancellationToken cancellationToken = default
    );
}
