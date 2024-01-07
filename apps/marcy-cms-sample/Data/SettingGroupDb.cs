namespace CandyKingdom.MarcyCms.Sample.Data;

public sealed class SettingGroupDb
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public ICollection<SettingRecordDb> Records { get; private set; }

    public SettingGroupDb(int id, string title, ICollection<SettingRecordDb> records)
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
