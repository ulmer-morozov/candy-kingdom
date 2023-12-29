namespace CandyKingdom.Marcy.Storage;

public sealed class LocalFileStorage : IFileStorage
{
  private readonly string _outputDir;
  private readonly string _urlPrefix;

  public LocalFileStorage(string urlPrefix, string outputDir)
  {
    _urlPrefix = urlPrefix;
    _outputDir = outputDir;
  }

  public async Task<StoredFile> Store(
    Stream stream,
    string filename,
    string mimeType,
    CancellationToken cancellationToken = default
  )
  {
    if (!Directory.Exists(_outputDir))
      Directory.CreateDirectory(_outputDir);

    var filePath = Path.Combine(_outputDir, filename);

    await using var fileStream = File.Create(filePath);
    await stream.CopyToAsync(fileStream, cancellationToken);

    var file = new StoredFile { Url = $"{_urlPrefix}{filename}" };
    return file;
  }
}
