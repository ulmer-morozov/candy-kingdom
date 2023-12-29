// By ryanholden8
// https://stackoverflow.com/questions/63813872/record-types-with-collection-properties-collections-with-value-semantics

namespace CandyKingdom.Marcy.Immutables;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class JsonConverterForImmutableList2Factory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType
        && typeToConvert.GetGenericTypeDefinition() == typeof(ImmutableList2<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];

        var arrayType = typeof(JsonConverterForImmutableList2<>);

        var converter = (JsonConverter)
            Activator.CreateInstance(
                arrayType.MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null
            )!;

        return converter;
    }

    private class JsonConverterForImmutableList2<T> : JsonConverter<ImmutableList2<T>>
    {
        public override ImmutableList2<T> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            reader.Read();

            List<T> elements = new();

            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var value = JsonSerializer.Deserialize<T>(ref reader, options);

                if (value is not null)
                {
                    elements.Add(value);
                }

                reader.Read();
            }

            return elements.ToImmutableList2();
        }

        public override void Write(
            Utf8JsonWriter writer,
            ImmutableList2<T> value,
            JsonSerializerOptions options
        )
        {
            JsonSerializer.Serialize(writer, value.AsEnumerable(), options);
        }
    }
}
