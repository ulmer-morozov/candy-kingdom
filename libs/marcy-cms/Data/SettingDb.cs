using CandyKingdom.MarcyCms.Settings;

namespace CandyKingdom.MarcyCms.Data;

public sealed class SettingDb
{
    public Guid Id { get; private set; }
    public int Order { get; private set; }
    public string Title { get; private set; }
    public SettingData Data { get; private set; }
    public SettingGroupDb Group { get; private set; }

    public SettingDb(Guid id, int order, string title, SettingData data, SettingGroupDb group)
    {
        Id = id;
        Order = order;
        Title = title;
        Data = data;
        Group = group;
    }

    private SettingDb()
    {
        Title = null!;
        Group = null!;
        Data = null!;
    }

    public void SetData(SettingData newData)
    {
        Data = newData;
    }
}
