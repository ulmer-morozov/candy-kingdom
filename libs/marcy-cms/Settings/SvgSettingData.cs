using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record SvgSettingData : SettingData
{
    public const string SettingDataType = $"{MarcyConstants.LibPrefix}-svg";

    public FileSrc<ImageMeta> Src { get; init; } = new FileSrc<ImageMeta> { Meta = ImageMeta.Empty, Url = "", MimeType = "" };

    public SvgSettingData()
        : base(SettingDataType)
    {
    }
}

