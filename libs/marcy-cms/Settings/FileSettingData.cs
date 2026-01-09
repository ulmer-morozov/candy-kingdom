using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record FileSettingData : SettingData
{
    public const string SettingDataType = $"{MarcyConstants.LibPrefix}-file";

    public ImmutableList2<string> AllowedMimeTypes { get; init; } = ImmutableList2<string>.Empty;

    public FileSrc<FileMeta> Src { get; init; } = new FileSrc<FileMeta> { Meta = new() { ByteCount = 0 }, Url = "", MimeType = "" };

    public FileSettingData()
        : base(SettingDataType)
    {
    }
}

