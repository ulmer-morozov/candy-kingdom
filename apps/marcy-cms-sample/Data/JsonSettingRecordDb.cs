using System.Text.Json;

namespace CandyKingdom.MarcyCms.Sample.Data;

public sealed class JsonSettingRecordDb : SettingRecordDb<JsonSettingRecordDb>
{
    public string Json { get; private set; }

    public JsonSettingRecordDb(Guid id, string title, SettingGroupDb group, string? json = null)
        : base(id, title, SettingRecordType.Json, group)
    {
        Json = json ?? "";
    }

    private JsonSettingRecordDb()
    {
        Json = null!;
    }

    public void Set<T>(T value)
    {
        Json = Serialize(value);
    }

    public T? Get<T>()
    {
        return Deserialize<T>(Json);
    }

    public static JsonSettingRecordDb New<T>(Guid id, string title, SettingGroupDb group, T obj)
    {
        var record = new JsonSettingRecordDb(id, title, group);
        record.Set(obj);

        return record;
    }

    private static T? Deserialize<T>(string sourceJson)
    {
        var content = JsonSerializer.Deserialize<T>(sourceJson, JsonSettings);
        return content;
    }

    private static string Serialize<T>(T value)
    {
        var json = JsonSerializer.Serialize(value, JsonSettings);
        return json;
    }

    protected override void CopyInternal(JsonSettingRecordDb other)
    {
        Json = other.Json;
    }

    private static JsonSerializerOptions JsonSettings { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
