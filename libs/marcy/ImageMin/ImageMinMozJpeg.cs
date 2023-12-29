using System.Collections.Immutable;
using System.Runtime.InteropServices;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinMozJpeg : ImageMinBinVendor<ImageMinMozJpegOptions>
{
  private const string MozJpegBinVersion = "7.0.0";
  private static readonly string BaseUrl =
    $"https://raw.githubusercontent.com/imagemin/mozjpeg-bin/v{MozJpegBinVersion}/vendor/";

  public ImageMinMozJpeg(ImageMinMozJpegOptions? defaultOptions = null)
    : base(
      name: "mozjpeg",
      executable: new BinWrapper(
        executableNames: new[]
        {
          new OsDependendName("cjpeg.exe", OSPlatform.Windows, x64: true),
          new OsDependendName("cjpeg", OSPlatform.OSX, x64: true),
          new OsDependendName("cjpeg", OSPlatform.Linux, x64: true)
        },
        sources: new[]
        {
          new OsDependendSource($"{BaseUrl}macos/cjpeg", "cjpeg", OSPlatform.OSX, x64: true),
          new OsDependendSource($"{BaseUrl}linux/cjpeg", "cjpeg", OSPlatform.Linux, x64: true),
          new OsDependendSource(
            $"{BaseUrl}win/cjpeg.exe",
            "cjpeg.exe",
            OSPlatform.Windows,
            x64: true
          ),
        }
      ),
      validFormats: new[] { ImageFormat.Jpeg, ImageFormat.Png },
      defaultOptions: defaultOptions ?? new ImageMinMozJpegOptions()
    ) { }

  protected override ImmutableList<string> GetArgs(
    string input,
    string output,
    ImageMinMozJpegOptions options
  )
  {
    var args = new List<string> { "-outfile", $"\"{output}\"" };

    if (options.Quality != 0)
    {
      args.Add("-quality");
      args.Add($"{options.Quality}");
    }

    if (options.MaxMemory != 0)
    {
      args.Add("-maxmemory");
      args.Add($"{options.MaxMemory}");
    }

    args.Add(input);

    return args.ToImmutableList();
  }
}
