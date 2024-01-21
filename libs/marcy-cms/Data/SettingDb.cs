using CandyKingdom.MarcyCms.Settings;

namespace CandyKingdom.MarcyCms.Data;

public sealed class SettingDb
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public SettingData Data { get; private set; }
    public SettingGroupDb Group { get; private set; }

    public SettingDb(Guid id, string title, SettingGroupDb group, SettingData data)
    {
        Id = id;
        Title = title;
        Group = group;
        Data = data;
    }

    private SettingDb()
    {
        Title = null!;
        Group = null!;
        Data = null!;
    }
}
