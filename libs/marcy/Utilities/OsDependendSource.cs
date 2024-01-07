using System.Runtime.InteropServices;

namespace CandyKingdom.Marcy.Utilities;

public sealed record OsDependendSource
{
    public string Url { get; }
    public string FileName { get; }
    public OSPlatform Platform { get; }
    public bool x64 { get; }

    public OsDependendSource(string url, string fileName, OSPlatform platform, bool x64)
    {
        Url = url;
        Platform = platform;
        this.x64 = x64;
        FileName = fileName;
    }
}
