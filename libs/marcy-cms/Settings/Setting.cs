namespace CandyKingdom.MarcyCms.Settings;

public sealed record Setting<T>
{
    public required T Data { get; init; }
}

public record Setting
{
    public Guid Id { get; init; }
    public string Title { get; init; } = "";
}
