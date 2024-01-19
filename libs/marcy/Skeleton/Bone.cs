namespace CandyKingdom.Marcy.Skeleton;

public record Bone
{
    public string Style { get; init; } = "";

    public string MediaQuery { get; init; } = "";

    public bool Enabled { get; init; } = true;

    public string Type { get; }

    public Bone(string type)
    {
        Type = type;
    }
}
