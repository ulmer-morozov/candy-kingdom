using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.MarcyCms.Settings;

public sealed record SettingGroup
{
    public Guid Id { get; init; }
    public string Title { get; init; } = "";
    public ImmutableList2<Setting> Records { get; init; } = ImmutableList2<Setting>.Empty;
}
