using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record LottieSettingData : SettingData
{
    public const string SettingDataType = $"{MarcyConstants.LibPrefix}-lottie";

    public FileSrc<FileMeta> Src { get; init; } = new FileSrc<FileMeta> { Meta = new() { ByteCount = 0 }, Url = "", MimeType = "" };

    public LottieSettingData()
        : base(SettingDataType)
    {
    }
}

