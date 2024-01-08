namespace CandyKingdom.Marcy;

public static class LocalizedStringHelpers
{

    public static LocalizedString En(string en) => new(new Dictionary<string, string>() { [LocalizedString.EnCode] = en });
    public static LocalizedString Empty { get; } = LocalizedString.Empty;
}
