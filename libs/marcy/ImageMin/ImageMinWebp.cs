using System.Collections.Immutable;
using System.Runtime.InteropServices;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinWebp : ImageMinBinVendor<ImageMinWebpOptions>
{
  private const string BinVersion = "6.1.1";
  private static readonly string BaseUrl =
    $"https://raw.githubusercontent.com/imagemin/cwebp-bin/v{BinVersion}/vendor/";

  public ImageMinWebp(ImageMinWebpOptions? defaultOptions = null)
    : base(
      name: "cwebp",
      executable: new BinWrapper(
        executableNames: new[]
        {
          new OsDependendName("cwebp.exe", OSPlatform.Windows, x64: true),
          new OsDependendName("cwebp", OSPlatform.OSX, x64: true),
          new OsDependendName("cwebp", OSPlatform.Linux, x64: true)
        },
        sources: new[]
        {
          new OsDependendSource($"{BaseUrl}osx/cwebp", "cwebp", OSPlatform.OSX, x64: true),
          new OsDependendSource($"{BaseUrl}linux/x64/cwebp", "cwebp", OSPlatform.Linux, x64: true),
          new OsDependendSource(
            $"{BaseUrl}win/x64/cwebp.exe",
            "cwebp.exe",
            OSPlatform.Windows,
            x64: true
          ),
        }
      ),
      validFormats: new[] { ImageFormat.Webp },
      defaultOptions: defaultOptions ?? new ImageMinWebpOptions()
    ) { }

  protected override ImmutableList<string> GetArgs(
    string input,
    string output,
    ImageMinWebpOptions options
  )
  {
    var args = new List<string> { "-o", $"\"{output}\"" };

    if (options.Quality != 0)
    {
      args.Add("-q");
      args.Add($"{options.Quality}");
    }

    if (options.Quiet)
      args.Add("-quiet");

    args.Add(input);

    return args.ToImmutableList();
  }
}
