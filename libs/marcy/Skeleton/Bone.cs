namespace CandyKingdom.Marcy.Skeleton;

public abstract record Bone
{
    public string Style { get; init; } = "";

    public string MediaQuery { get; init; } = "";

    public bool Enabled { get; init; } = true;

    public abstract string Type { get; }
}
