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
        _validFormats = validFormats?.ToImmutableList() ?? ImmutableList<ImageFormat>.Empty;
        _executable = executable;
    }

    public override bool CanBeApplyed(string format)
    {
        var canBeApplyed = _validFormats.Any(
            x => string.Equals(x.Extension, format, StringComparison.InvariantCultureIgnoreCase)
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
            throw new Exception($"Невозможно минифицировать. Файл не найден: {input}");

        var args = GetArgs(input, output, options);

        await _executable.Run(args, cancellationToken);

        if (!File.Exists(output))
            throw new Exception($"Файл не минифицирован: {output}");
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
