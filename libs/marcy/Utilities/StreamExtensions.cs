using System.Security.Cryptography;
using System.Text;

namespace CandyKingdom.Marcy.Utilities;

public static class StreamExtensions
{
    public static async Task<string> CalcSha1AsBase62Async(this MemoryStream stream)
    {
        using var algorythm = MD5.Create();

        stream.Seek(0, SeekOrigin.Begin);
        var hashValue = await algorythm.ComputeHashAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);

        var base62 = Base62Encoding.ToString(hashValue);
        return base62;
    }

    public static async Task<string> CalcMd5AsBase62Async(
      this Stream stream,
      CancellationToken cancellationToken = default
    )
    {
        using var algorythm = MD5.Create();

        if (stream is MemoryStream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        var hashValue = await algorythm.ComputeHashAsync(stream, cancellationToken);

        if (stream is MemoryStream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        var base62 = Base62Encoding.ToString(hashValue);
        return base62;
    }

    public static string CalcMd5AsBase62(this string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var hashValue = MD5.HashData(bytes);
        var base62 = Base62Encoding.ToString(hashValue);
        return base62;
    }
}
