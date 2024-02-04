namespace CandyKingdom.Marcy.ImageTools;

public record MediaUploaderBaseConfig
{
    public required DirectoryInfo CacheDir { get; init; } // todo: do we really need this? may be just use some general temp folder?
}
