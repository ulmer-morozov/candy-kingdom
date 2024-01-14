namespace CandyKingdom.Marcy.ImageTools;

public record MediaUploaderBaseConfig
{
    public required DirectoryInfo CacheDir { get; init; }
}
