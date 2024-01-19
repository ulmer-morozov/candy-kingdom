using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CandyKingdom.Marcy;

public static class LocalizedStringHelpers
{
    public static LocalizedString En(string en) => new(new Dictionary<string, string>() { [LocalizedString.EnCode] = en });
    public static LocalizedString Empty { get; } = LocalizedString.Empty;

    public static readonly JsonSerializerOptions DefaultSerializerOptions = NewLocalizedStringJsonOptions();

    public static JsonSerializerOptions NewLocalizedStringJsonOptions()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        };

        options.Converters.Add(new JsonConverterForLocalizedString());

        return options;
    }

    public static string ToJson(this LocalizedString ls, JsonSerializerOptions? options = null) => JsonSerializer.Serialize(ls, options ?? DefaultSerializerOptions);

    public static LocalizedString FromJson(string lsJson, JsonSerializerOptions? options = null) => JsonSerializer.Deserialize<LocalizedString>(lsJson, options ?? DefaultSerializerOptions) ?? LocalizedString.Empty;
}
