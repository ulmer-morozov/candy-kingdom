using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record LocalizedTextSettingData : SettingData
{
    public const string TYPE = $"{MarcyConstants.LibPrefix}-localized-text";
    public LocalizedString Text { get; init; } = LocalizedString.Empty;
    public TextSettingType TextType { get; init; } = TextSettingType.SingleLine;

    public LocalizedTextSettingData()
        : base(TYPE)
    {
    }
}
