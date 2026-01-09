using System.Text.Json.Serialization;

namespace CandyKingdom.MarcyCms.Settings;

public interface ISetting<out T>
    where T : SettingData
{
    public Guid Id { get; }
    public string Title { get; }
    public T Data { get; }
}

public sealed record Setting<T> : Setting, ISetting<T>
    where T : SettingData
{
    public required T Data { get; init; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Setting<SettingData>), "data")]
public record Setting
{
    public Guid Id { get; init; }
    public int Order { get; init; }
    public string Title { get; init; } = "";

    public static Setting<T> NewFromData<T>(T data)
        where T : SettingData
    {
        return new Setting<T>
        {
            Data = data
        };
    }
}

[JsonSerializable(typeof(Setting))]
[JsonSerializable(typeof(Setting<SettingData>))]
public partial class MarcyCmsJsonContext : JsonSerializerContext
{
}
