namespace CandyKingdom.Marcy;

public sealed class DefferedStreamClone : IDisposable, IAsyncDisposable
{
  private readonly Func<Stream> _sourceStreamFactory;
  private MemoryStream? msStream;

  public DefferedStreamClone(Func<Stream> sourceStreamFactory)
  {
    _sourceStreamFactory = sourceStreamFactory;
  }

  public MemoryStream Value
  {
    get
    {
      if (msStream != null)
        return msStream;

      msStream = new MemoryStream();

      using var srcStream = _sourceStreamFactory();

      srcStream.CopyTo(msStream);
      msStream.Seek(0, SeekOrigin.Begin);

      return msStream;
    }
  }

  public void Dispose()
  {
    msStream?.Dispose();
  }

  public async ValueTask DisposeAsync()
  {
    if (msStream != null)
      await msStream.DisposeAsync();
  }
}
