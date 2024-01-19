using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms.Data;

public sealed class SettingDb : JsonDataOwnerDb<PageData>
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public SettingGroupDb Group { get; private set; }

    public SettingDb(Guid id, string title, SettingGroupDb group)
    {
        Id = id;
        Title = title;
        Group = group;
    }

    private SettingDb()
    {
        Title = null!;
        Group = null!;
    }
}
