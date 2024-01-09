using System.Collections.Immutable;

namespace CandyKingdom.MarcyCms.Sample.Core;

public static class SomeMimeTypes
{
    public static ImmutableList<string> Images { get; } = ["image/png", "image/jpeg"];
    public static ImmutableList<string> Videos { get; } = ["video/mp4"];
}
