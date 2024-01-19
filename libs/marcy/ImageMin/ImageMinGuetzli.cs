using System.Collections.Immutable;
using System.Runtime.InteropServices;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinGuetzli : ImageMinBinVendor<ImageMinGuetzliOptions>
{
    private const string GuetzliBinVersion = "5.0.0";
    private static readonly string BaseUrl =
      $"https://raw.githubusercontent.com/imagemin/guetzli-bin/v{GuetzliBinVersion}/vendor/";

    public ImageMinGuetzli(ImageMinGuetzliOptions? defaultOptions = null)
      : base(
        name: "guetzli",
        executable: new BinWrapper(
          executableNames:
          [
          new OsDependendName("guetzli", OSPlatform.OSX, x64: true),
          new OsDependendName("guetzli", OSPlatform.Linux, x64: true),
          new OsDependendName("guetzli.exe", OSPlatform.Windows, x64: true)
          ],
          sources:
          [
          new OsDependendSource($"{BaseUrl}macos/guetzli", "guetzli", OSPlatform.OSX, x64: true),
          new OsDependendSource($"{BaseUrl}linux/guetzli", "guetzli", OSPlatform.Linux, x64: true),
          new OsDependendSource(
            $"{BaseUrl}win/guetzli.exe",
            "guetzli.exe",
            OSPlatform.Windows,
            x64: true
          )
          ]
        ),
        validFormats: [ImageFormat.Jpeg, ImageFormat.Png],
        defaultOptions: defaultOptions ?? new ImageMinGuetzliOptions()
      )
    { }

    protected override ImmutableList<string> GetArgs(
      string input,
      string output,
      ImageMinGuetzliOptions options
    )
    {
        var args = new List<string>();

        if (options.Quality != 0)
        {
            args.Add("--quality");
            args.Add($"{options.Quality}");
        }

        if (options.MemoryLimit != 0)
        {
            args.Add("--memlimit");
            args.Add($"{options.MemoryLimit}");
        }

        args.Add($"\"{input}\"");
        args.Add($"\"{output}\"");

        return args.ToImmutableList();
    }
}
