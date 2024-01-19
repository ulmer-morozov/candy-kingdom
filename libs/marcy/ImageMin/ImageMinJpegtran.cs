using System.Collections.Immutable;
using System.Runtime.InteropServices;

using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageMin;

public sealed class ImageMinJpegtran : ImageMinBinVendor<ImageMinJpegtranOptions>
{
    private const string JpegtranBinVersion = "5.0.2";
    private static readonly string BaseUrl =
      $"https://raw.githubusercontent.com/imagemin/jpegtran-bin/v{JpegtranBinVersion}/vendor/";

    public ImageMinJpegtran(ImageMinJpegtranOptions? defaultOptions = null)
      : base(
        name: "jpegtran",
        executable: new BinWrapper(
          executableNames:
          [
        new OsDependendName("jpegtran", OSPlatform.OSX, x64: true),
              new OsDependendName("jpegtran", OSPlatform.Linux, x64: true),
              new OsDependendName("jpegtran.exe", OSPlatform.Windows, x64: true),
          ],
          sources:
          [
        new OsDependendSource($"{BaseUrl}macos/jpegtran", "jpegtran", OSPlatform.OSX, x64: true),
              new OsDependendSource(
          $"{BaseUrl}linux/x64/jpegtran",
          "jpegtran",
          OSPlatform.Linux,
          x64: true
        ),
              new OsDependendSource(
          $"{BaseUrl}win/x64/jpegtran.exe",
          "jpegtran.exe",
          OSPlatform.Windows,
          x64: true
        ),
              new OsDependendSource(
          $"{BaseUrl}win/x64/libjpeg-62.dll",
          "libjpeg-62.dll",
          OSPlatform.Windows,
          x64: true
        ),
          ]
        ),
        validFormats: [ImageFormat.Jpeg, ImageFormat.Png],
        defaultOptions: defaultOptions ?? new ImageMinJpegtranOptions()
      )
    { }

    protected override ImmutableList<string> GetArgs(
      string input,
      string output,
      ImageMinJpegtranOptions options
    )
    {
        var args = new List<string> { "-outfile", $"\"{output}\"" };

        if (options.Progressive)
        {
            args.Add("-progressive");
        }

        args.Add(input);

        return [.. args];
    }
}
