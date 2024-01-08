namespace CandyKingdom.MarcyCms.Data;

public abstract class SettingRecordDb<T> : SettingRecordDb
    where T : SettingRecordDb
{
    protected abstract void CopyInternal(T other);

    public SettingRecordDb(Guid id, string title, SettingRecordType type, SettingGroupDb group)
        : base(id, title, type, group)
    {

    }

    protected SettingRecordDb()
    {

    }

    public void Copy(T record)
    {
        base.Copy(record);
        CopyInternal(record);
    }
}

public abstract class SettingRecordDb
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public SettingGroupDb Group { get; private set; }
    public SettingRecordType RecordType { get; private set; }

    public SettingRecordDb(Guid id, string title, SettingRecordType type, SettingGroupDb group)
    {
        Id = id;
        Title = title;
        RecordType = type;
        Group = group;
    }

    protected SettingRecordDb()
    {
        Title = null!;
        Group = null!;
    }

    protected void Copy(SettingRecordDb record)
    {
        Title = record.Title;
    }
}
