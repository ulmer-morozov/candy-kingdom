using CandyKingdom.Marcy;
using CandyKingdom.MarcyCms.Data;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record LocalizedTextSettingData : SettingData
{
    public const string TYPE = $"{MarcyConstants.LibPrefix}-localized-text";
    public string Text { get; init; } = "";
    public TextSettingType TextType { get; init; } = TextSettingType.SingleLine;

    public LocalizedTextSettingData()
        : base(TYPE)
    {
    }
}
