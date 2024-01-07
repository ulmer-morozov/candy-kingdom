namespace CandyKingdom.Marcy.ImageMin;

public abstract class ImageMinVendor
{
    public string Name { get; }

    protected ImageMinVendor(string name)
    {
        Name = name;
    }

    public abstract Task Minify(
      string input,
      string output,
      CancellationToken cancellationToken = default
    );
    public abstract bool CanBeApplyed(string format);

    public async Task<MemoryStream> Minify(
      string input,
      CancellationToken cancellationToken = default
    )
    {
        var inputFile = new FileInfo(input);

        var tempDirPath = Path.GetTempPath();

        var guid = Guid.NewGuid().ToString().Replace("-", "");
        var tempFileName = $"minify-{Name}-{guid}-{inputFile.Name}";

        var tempFilePath = Path.Combine(tempDirPath, tempFileName);

        await Minify(input, tempFilePath);

        var ms = new MemoryStream();

        await using (var file = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
            await file.CopyToAsync(ms, cancellationToken);

        File.Delete(tempFilePath);
        return ms;
    }
}
