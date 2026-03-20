namespace CandyKingdom.Marcy.Storage;

public sealed class LocalFileStorage : IFileStorage
{
    public string OutputDir { get; }
    public string UrlPrefixutDir { get; }

    public LocalFileStorage(string urlPrefix, string outputDir)
    {
        UrlPrefixutDir = urlPrefix;
        OutputDir = outputDir;

        Console.WriteLine($"FILE STORAGE\n prefix – {urlPrefix}\n OutputDir – {OutputDir}");
    }

    public async Task<StoredFile> Store(
      Stream stream,
      string name,
      string mimeType,
      CancellationToken cancellationToken = default
    )
    {
        var filePath = GetStoredFilePath(name);

        await using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream, cancellationToken);

        var file = new StoredFile { Url = $"{UrlPrefixutDir}{name}" };
        return file;
    }

    public string GetStoredFilePath(string fileName)
    {
        if (!Directory.Exists(OutputDir))
        {
            Directory.CreateDirectory(OutputDir);
        }

        var filePath = Path.Combine(OutputDir, fileName);
        return filePath;
    }
}
