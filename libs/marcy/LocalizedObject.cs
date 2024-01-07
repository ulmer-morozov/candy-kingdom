using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.Marcy;

public record LocalizedObject<T>
{
    protected const string RuCode = "ru";
    protected const string EnCode = "en";

    public ImmutableDictionary2<string, T> Localizations { get; init; }

    public LocalizedObject(ImmutableDictionary2<string, T> localizations)
    {
        Localizations = localizations;
    }

    public LocalizedObject(ICollection<KeyValuePair<string, T>>? localizations = null)
      : this(
        localizations as ImmutableDictionary2<string, T>
          ?? localizations?.ToImmutableDictionary2()
          ?? ImmutableDictionary2<string, T>.Empty
      )
    { }

    public LocalizedObject<T> AddRange(ICollection<KeyValuePair<string, T>> newValues)
    {
        var newLocalizations = Localizations.AddRange(newValues);
        return this with { Localizations = newLocalizations };
    }

    public T Get(string lang, T defaultValue)
    {
        if (!Localizations.ContainsKey(lang))
            return defaultValue;

        return Localizations[lang];
    }

    public bool IsEmpty()
    {
        return !Localizations.Any();
    }
}
