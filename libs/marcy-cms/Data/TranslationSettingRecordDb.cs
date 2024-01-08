using CandyKingdom.Marcy;

namespace CandyKingdom.MarcyCms.Data;

public sealed class TranslationSettingRecordDb : SettingRecordDb<TranslationSettingRecordDb>
{
    public LocalizedString Text { get; private set; }
    public TextSettingRecordType TextType { get; private set; }

    public TranslationSettingRecordDb(Guid id, string title, LocalizedString text, TextSettingRecordType textType, SettingGroupDb group)
        : base(id, title, SettingRecordType.Translation, group)
    {
        Text = text;
        TextType = textType;
    }

    private TranslationSettingRecordDb()
    {
        Text = null!;
    }

    protected override void CopyInternal(TranslationSettingRecordDb other)
    {
        Text = other.Text;
    }
}
