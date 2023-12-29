namespace CandyKingdom.Marcy.ImageTools;

public sealed class InMemoryImage : InMemoryFile<ImageMeta>
{
    public string PreviewBase64 { get; init; } = "";
}
