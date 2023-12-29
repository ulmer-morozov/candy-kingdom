using System.Text.Json.Serialization;
using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public sealed record LocalizedString : LocalizedObject<string>
{
  [JsonIgnore]
  public string En => GetString(EnCode);

  [JsonIgnore]
  public string Ru => GetString(RuCode);

  public LocalizedString(ImmutableDictionary2<string, string> localizations)
      : base(localizations) { }

  public LocalizedString(IDictionary<string, string>? localizations = null)
      : base(localizations) { }

  public string GetString(string Code) => Get(Code, string.Empty);

  #region Static Helpers

  public static LocalizedString Empty { get; } = new LocalizedString();

  public static LocalizedString From(string en, string ru) =>
      new(new Dictionary<string, string>() { [EnCode] = en, [RuCode] = ru });

  public static LocalizedString FromEn(string en) =>
      new(new Dictionary<string, string>() { [EnCode] = en });

  public static LocalizedString Combine(
      LocalizedString ls1,
      LocalizedString ls2,
      char delimiter,
      bool trim
  )
  {
    var combinedLocalizations = new Dictionary<string, string>();
    var combinedKeys = ls1.Localizations.Keys.Concat(ls2.Localizations.Keys);

    foreach (var key in combinedKeys)
    {
      var val1 = ls1.GetString(key);
      var val2 = ls2.GetString(key);

      var combinedValue = Combine(val1, val2, delimiter, trim);

      combinedLocalizations.Add(key, combinedValue);
    }

    return new LocalizedString(combinedLocalizations);
  }

  public static LocalizedString Combine(
      char delimiter,
      bool trim,
      params LocalizedString[] strings
  )
  {
    return strings.Aggregate(
        Empty,
        (total, next) => Combine(total, next, delimiter: delimiter, trim: trim)
    );
  }

  private static string Combine(string s1, string s2, char delimiter, bool trim)
  {
    if (string.IsNullOrEmpty(s1))
      return s2;

    if (string.IsNullOrEmpty(s2))
      return s1;

    var result = trim
        ? $"{s1.TrimEnd(delimiter)}{delimiter}{s2.TrimStart(delimiter)}"
        : $"{s1}{delimiter}{s2}";

    return result;
  }

  #endregion
}
