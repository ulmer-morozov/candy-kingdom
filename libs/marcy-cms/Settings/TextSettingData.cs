using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record TextSettingData : SettingData
{
    public const string TYPE = $"{MarcyConstants.LibPrefix}-text";
    public string Text { get; init; } = "";
    public TextSettingType TextType { get; init; } = TextSettingType.SingleLine;

    public TextSettingData()
        : base(TYPE)
    {
    }
}
