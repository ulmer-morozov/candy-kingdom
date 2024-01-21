namespace CandyKingdom.Marcy.Pages;

public record PageData
{
    public string Type { get; }

    public PageData(string type)
    {
        Type = type;
    }

    public static PageData Empty { get; } = new PageData("");
}
