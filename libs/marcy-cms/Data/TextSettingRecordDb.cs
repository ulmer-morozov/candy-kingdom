namespace CandyKingdom.MarcyCms.Data;

public sealed class TextSettingRecordDb : SettingRecordDb<TextSettingRecordDb>
{
    public string Text { get; private set; }
    public TextSettingRecordType TextType { get; private set; }

    public TextSettingRecordDb(Guid id, string title, string text, TextSettingRecordType textType, SettingGroupDb group)
        : base(id, title, SettingRecordType.Text, group)
    {
        Text = text;
        TextType = textType;
    }

    private TextSettingRecordDb()
    {
        Text = null!;
    }

    public void SetText(string newText)
    {
        Text = newText;
    }

    protected override void CopyInternal(TextSettingRecordDb other)
    {
        Text = other.Text?.Trim() ?? string.Empty;
    }
}
