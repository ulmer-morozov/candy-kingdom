using System.Collections.Immutable;

using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Utilities;

namespace CandyKingdom.Marcy.ImageMin;

public abstract class ImageMinBinVendor<T> : ImageMinVendor
  where T : class
{
    private readonly T _defaultOptions;
    private readonly BinWrapper _executable;
    private readonly ImmutableList<ImageFormat> _validFormats;

    protected abstract ImmutableList<string> GetArgs(string input, string output, T options);

    protected ImageMinBinVendor(
      string name,
      BinWrapper executable,
      T defaultOptions,
      IEnumerable<ImageFormat> validFormats
    )
      : base(name)
    {
        _defaultOptions = defaultOptions ?? throw new ArgumentNullException(nameof(defaultOptions));
        _validFormats = validFormats?.ToImmutableList() ?? [];
        _executable = executable;
    }

    public override bool CanBeApplyed(string format)
    {
        var canBeApplyed = _validFormats.Any(
          x => string.Equals(x.Extension, format, StringComparison.OrdinalIgnoreCase)
        );
        return canBeApplyed;
    }

    public async Task Minify(
      string input,
      string output,
      T options,
      CancellationToken cancellationToken = default
    )
    {
        input = Path.GetFullPath(input);
        output = Path.GetFullPath(output);

        if (!File.Exists(input))
        {
            throw new Exception($"Couldn't minify file. File's not found: {input}");
        }

        var args = GetArgs(input, output, options);

        await _executable.Run(args, cancellationToken);

        if (!File.Exists(output))
        {
            File.Copy(input, output);
            Console.WriteLine($"File wasn't minified: {output}");
        }
    }

    public override Task Minify(
      string input,
      string output,
      CancellationToken cancellationToken = default
    )
    {
        return Minify(input, output, _defaultOptions, cancellationToken);
    }
}
