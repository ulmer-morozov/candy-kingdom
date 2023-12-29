using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageTools;

public class TempFile<T> : TempFile
{
  public required T Meta { get; init; }
}

public class TempFile : IDisposable
{
  public required FileFormat Format { get; init; }
  public required FileInfo File { get; init; }

  public async Task<string> CalcMd5AsBase62(CancellationToken cancellationToken = default)
  {
    using var fileStream = new FileStream(File.FullName, FileMode.Open, FileAccess.Read);
    var hash = await fileStream.CalcMd5AsBase62Async(cancellationToken: cancellationToken);

    return hash;
  }

  public void Dispose()
  {
    File.Delete();
    GC.SuppressFinalize(this);
  }
}
