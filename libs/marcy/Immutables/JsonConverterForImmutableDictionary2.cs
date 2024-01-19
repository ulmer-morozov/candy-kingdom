// By ryanholden8
// https://stackoverflow.com/questions/63813872/record-types-with-collection-properties-collections-with-value-semantics

using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy.Immutables;

public sealed class JsonConverterForImmutableDictionary2Factory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
      typeToConvert.IsGenericType
      && typeToConvert.GetGenericTypeDefinition() == typeof(ImmutableDictionary2<,>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var keyType = typeToConvert.GetGenericArguments()[0];
        var valueType = typeToConvert.GetGenericArguments()[1];

        var dictionaryType = typeof(JsonConverterForImmutableDictionary2<,>);

        var converter = (JsonConverter)
          Activator.CreateInstance(
            dictionaryType.MakeGenericType(keyType, valueType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: null,
            culture: null
          )!;

        return converter;
    }

    private sealed class JsonConverterForImmutableDictionary2<TKey, TValue>
      : JsonConverter<ImmutableDictionary2<TKey, TValue>>
      where TKey : notnull
    {
        public override ImmutableDictionary2<TKey, TValue> Read(
          ref Utf8JsonReader reader,
          Type typeToConvert,
          JsonSerializerOptions options
        )
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            // reader.Read();

            var newDict = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options) ?? throw new JsonException("Cannot read dict");

            return newDict.ToImmutableDictionary2();
        }

        public override void Write(
          Utf8JsonWriter writer,
          ImmutableDictionary2<TKey, TValue> value,
          JsonSerializerOptions options
        )
        {
            var orderedDict = value.OrderBy(x => x.Key).ToImmutableDictionary();

            JsonSerializer.Serialize(writer, orderedDict, options);
        }
    }
}
