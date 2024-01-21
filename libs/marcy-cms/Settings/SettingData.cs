namespace CandyKingdom.MarcyCms.Settings;

public record SettingData
{
    public string Type { get; }

    public SettingData(string type)
    {
        Type = type;
    }

    public static SettingData Empty { get; } = new("");
}
