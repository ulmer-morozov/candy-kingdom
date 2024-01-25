namespace CandyKingdom.Marcy;

public static class LocalizedStringHelpers
{
    public static LocalizedString En(string text)
    {
        return Text(LocalizedString.EnCode, text);
    }

    public static LocalizedString Text(string locale, string text)
    {
        return new(new Dictionary<string, string>() { [locale] = text });
    }

    public static LocalizedString Empty { get; } = LocalizedString.Empty;
}
