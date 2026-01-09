using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms;

public sealed record GetChildrenParams
{
    public PublishStatus PublishStatus { get; init; } = PublishStatus.NotSet;
}
