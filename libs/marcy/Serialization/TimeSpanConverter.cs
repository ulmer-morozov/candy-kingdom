using System.Text.Json;
using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy.Serialization;

public sealed class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public static TimeSpanConverter Instance { get; } = new();

    public override TimeSpan Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options
    )
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.TotalMilliseconds);
    }
}
