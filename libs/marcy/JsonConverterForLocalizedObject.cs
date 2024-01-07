using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CandyKingdom.Marcy;

public sealed class JsonConverterForLocalizedObjectFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
      typeToConvert.IsGenericType
      && typeToConvert.GetGenericTypeDefinition() == typeof(LocalizedObject<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var keyType = typeToConvert.GetGenericArguments()[0];

        var type = typeof(JsonConverterForLocalizedObject<>);

        var converter = (JsonConverter)
          Activator.CreateInstance(
            type.MakeGenericType(keyType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: null,
            culture: null
          )!;

        return converter;
    }

    private sealed class JsonConverterForLocalizedObject<T> : JsonConverter<LocalizedObject<T>>
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
}
