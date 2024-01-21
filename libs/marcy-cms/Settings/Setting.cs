namespace CandyKingdom.MarcyCms.Settings;

public interface ISetting<out T>
{
    public T Data { get; }
}

public sealed record Setting<T> : Setting, ISetting<T>
    where T : SettingData
{
    public required T Data { get; init; }
}

public abstract record Setting
{
    public Guid Id { get; init; }
    public string Title { get; init; } = "";
}
