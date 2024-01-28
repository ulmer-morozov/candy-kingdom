using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms;

public sealed record GetPageParams
{
    public bool IncludeChildren { get; init; } = true;
    public PublishStatus PublishStatus { get; init; } = PublishStatus.NotSet;
}
