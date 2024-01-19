using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms.Sample.Bones;

public sealed record WebLink
{
    public required LocalizedString Title { get; init; }

    public required LocalizedString Href { get; init; }

    public static WebLink Empty { get; } =
        new WebLink { Title = LocalizedString.Empty, Href = LocalizedString.Empty };
}
