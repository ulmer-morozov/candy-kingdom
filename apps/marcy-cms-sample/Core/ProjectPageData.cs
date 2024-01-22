using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms.Sample.PageTypes;

public sealed record ProjectPageData : PageData
{
    public const string TYPE = "project";

    public LocalizedString SpecialTitle { get; init; } = LocalizedString.Empty;

    public ProjectPageData()
        : base(TYPE)
    {
    }
}
