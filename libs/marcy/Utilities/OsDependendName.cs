using System.Runtime.InteropServices;

namespace CandyKingdom.Marcy.Utilities;

public sealed record OsDependendName
{
    public string Name { get; }
    public OSPlatform Platform { get; }
    public bool x64 { get; }

    public OsDependendName(string name, OSPlatform platform, bool x64)
    {
        Name = name;
        Platform = platform;
        this.x64 = x64;
    }
}
