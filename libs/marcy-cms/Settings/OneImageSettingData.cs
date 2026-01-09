using CandyKingdom.Marcy;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record OneImageSettingData : SettingData
{
    public const string SettingDataType = $"{MarcyConstants.LibPrefix}-one-image";

    public int Width { get; init; }
    public int Height { get; init; }
    public ImageFormat Format { get; init; } = ImageFormat.Empty;
    public ImmutableList2<string> AllowedMimeTypes { get; init; } = ImmutableList2<string>.Empty;
    public FileSrc<ImageMeta> Src { get; init; } = new FileSrc<ImageMeta> { Meta = ImageMeta.Empty, Url = "", MimeType = "" };

    public OneImageSettingData()
        : base(SettingDataType)
    {
    }
}

