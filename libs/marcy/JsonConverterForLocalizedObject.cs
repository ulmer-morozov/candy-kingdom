using System.Text.Json;
using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy;

public sealed class JsonConverterForLocalizedObject<T> : JsonConverter<LocalizedObject<T>>
{
    public override LocalizedObject<T> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = JsonSerializer.Deserialize<Dictionary<string, T>>(ref reader, options);

        return new LocalizedObject<T>(value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        LocalizedObject<T> value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value.Localizations, options);
    }
}
