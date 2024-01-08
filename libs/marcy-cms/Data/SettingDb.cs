using System.Text.Json;

namespace CandyKingdom.MarcyCms.Data;

public sealed class SettingDb
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public SettingGroupDb Group { get; private set; }
    public string Json { get; private set; }

    public SettingDb(Guid id, string title, SettingGroupDb group)
    {
        Id = id;
        Title = title;
        Group = group;
        Json = "";
    }

    private SettingDb()
    {
        Title = null!;
        Group = null!;
        Json = null!;
    }

    public T Get<T>(JsonSerializerOptions? options)
        where T : SettingData, new()
    {
        if (string.IsNullOrWhiteSpace(Json))
            return new T();


        options ??= SerializerOptions;
        var obj = JsonSerializer.Deserialize<T>(Json, options);

        return obj ?? new T();
    }

    public void Set<T>(T data, JsonSerializerOptions? options)
       where T : SettingData, new()
    {
        options ??= SerializerOptions;
        Json = JsonSerializer.Serialize(data, options);
    }

    private static JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
