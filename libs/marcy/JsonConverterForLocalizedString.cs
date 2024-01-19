using System.Text.Json;
using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy;

public sealed class JsonConverterForLocalizedString : JsonConverter<LocalizedString>
{
    public override LocalizedString Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var localizedObjectConcerter =
            (JsonConverter<LocalizedObject<string>>)options.GetConverter(typeof(LocalizedObject<string>));

        var localizedObject = localizedObjectConcerter.Read(ref reader, typeToConvert, options) ?? throw new JsonException("Localized object cannot be null");

        return new LocalizedString(localizedObject.Localizations);
    }

    public override void Write(
        Utf8JsonWriter writer,
        LocalizedString value,
        JsonSerializerOptions options
    )
    {
        var localizedObjectConcerter =
            (JsonConverter<LocalizedObject<string>>)options.GetConverter(typeof(LocalizedObject<string>));

        localizedObjectConcerter.Write(writer, value, options);
    }
}
