namespace CandyKingdom.MarcyCms.Sample.Core;

public sealed record GetPageParams
{
    public bool IncludeChildren { get; init; } = true;
    public PublishStatus publishStatus { get; init; } = PublishStatus.NotSet;
}
