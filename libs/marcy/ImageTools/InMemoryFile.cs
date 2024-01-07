namespace CandyKingdom.Marcy.ImageTools;

public class InMemoryFile<T> : InMemoryFile
{
    public required T Meta { get; init; }
}

public class InMemoryFile : IDisposable
{
    public required FileFormat Format { get; init; }
    public required MemoryStream Stream { get; init; }

    public static Task<InMemoryFile> ReadFromStream(Stream stream, string mimeType, string extenstion)
    {
        return ReadFromStream(stream, new FileFormat { MimeType = mimeType, Extension = extenstion });
    }

    public static async Task<InMemoryFile> ReadFromStream(Stream stream, FileFormat format)
    {
        var msStream = new MemoryStream();

        await stream.CopyToAsync(msStream);
        msStream.Seek(0, SeekOrigin.Begin);

        return new InMemoryFile { Format = format, Stream = msStream };
    }

    public void Dispose()
    {
        Stream.Dispose();
        GC.SuppressFinalize(this);
    }
}
