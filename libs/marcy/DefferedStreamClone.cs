namespace CandyKingdom.Marcy;

public sealed class DefferedStreamClone : IDisposable, IAsyncDisposable
{
    private readonly Func<Stream> _sourceStreamFactory;
    private MemoryStream? _msStream;

    public DefferedStreamClone(Func<Stream> sourceStreamFactory)
    {
        _sourceStreamFactory = sourceStreamFactory;
    }

    public MemoryStream Value
    {
        get
        {
            if (_msStream != null)
                return _msStream;

            _msStream = new MemoryStream();

            using var srcStream = _sourceStreamFactory();

            srcStream.CopyTo(_msStream);
            _msStream.Seek(0, SeekOrigin.Begin);

            return _msStream;
        }
    }

    public void Dispose()
    {
        _msStream?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_msStream != null)
            await _msStream.DisposeAsync();
    }
}
