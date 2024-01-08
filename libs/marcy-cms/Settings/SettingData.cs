namespace CandyKingdom.MarcyCms.Data;

public record SettingData
{
    public string Type { get; }

    public SettingData(string type)
    {
        Type = type;
    }
}
