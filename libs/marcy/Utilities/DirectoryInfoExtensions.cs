namespace CandyKingdom.Marcy.Utilities;

public static class DirectoryInfoExtensions
{
    public static async Task<FileInfo> Store(
        this DirectoryInfo directory,
        Stream stream,
        string filename,
        CancellationToken cancellationToken = default
    )
    {
        var tempFilePath = Path.Combine(directory.FullName, filename);

        await using var file = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(file, cancellationToken);

        return new FileInfo(tempFilePath);
    }
}
