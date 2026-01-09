namespace CandyKingdom.Marcy.Pages;

public sealed record OpenGraphData
{
    public LocalizedString Title { get; init; } = LocalizedString.Empty;
    public LocalizedString Description { get; init; } = LocalizedString.Empty;
    public LocalizedObject<FileSrc<ImageMeta>> Image { get; init; } = new LocalizedObject<FileSrc<ImageMeta>>();

    public static OpenGraphData Empty { get; } = new();
}
