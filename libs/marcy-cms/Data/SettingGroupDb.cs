namespace CandyKingdom.MarcyCms.Data;

public sealed class SettingGroupDb
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public ICollection<SettingDb> Records { get; private set; }

    public SettingGroupDb(Guid id, string title, ICollection<SettingDb> records)
    {
        Id = id;
        Title = title;
        Records = records;
    }

    private SettingGroupDb()
    {
        Title = null!;
        Records = null!;
    }
}
