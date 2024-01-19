using System.Text.Json;

namespace CandyKingdom.MarcyCms.Data;

public abstract class JsonDataOwnerDb<TData>
{
    public string DataJson { get; protected set; } = "";

    public T Get<T>(JsonSerializerOptions? options)
        where T : TData, new()
    {
        if (string.IsNullOrWhiteSpace(DataJson))
        {
            return new T();
        }

        options ??= SerializerOptions;
        var obj = JsonSerializer.Deserialize<T>(DataJson, options);

        return obj ?? new T();
    }

    public void Set<T>(T data, JsonSerializerOptions? options)
       where T : TData, new()
    {
        options ??= SerializerOptions;
        DataJson = JsonSerializer.Serialize(data, options);
    }

    private static JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

}
