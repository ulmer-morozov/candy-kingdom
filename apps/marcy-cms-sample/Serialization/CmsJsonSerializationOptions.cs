using System.Collections.Immutable;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Serialization;
using CandyKingdom.MarcyCms.Settings;

namespace CandyKingdom.MarcyCms.Sample.Serialization;

public static class CmsJsonSerializationOptions
{
    public static readonly ImmutableList<JsonConverter> DefaultConverters = [
        new JsonStringEnumConverter(),
        new JsonConverterForLocalizedObject<string>(),
        new JsonConverterForLocalizedString()
    ];

    public static readonly IJsonTypeInfoResolver CombinedResolver = JsonTypeInfoResolver.Combine
    (
        new CmsSampleTypeInfoResolver(),
        MarcyJsonContext.Default,
        MarcyCmsJsonContext.Default
    );

    public static JsonSerializerOptions New()
    {
        return Configure(new JsonSerializerOptions());
    }

    public static JsonSerializerOptions Configure(JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic); // multi language serialization
        options.TypeInfoResolver = CombinedResolver;

        DefaultConverters.ForEach(options.Converters.Add);

        return options;
    }
}
