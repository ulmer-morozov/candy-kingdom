using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms.Sample.PageTypes;

public sealed record ProjectPageData : PageData
{
    public const string TYPE = "project";

    public ProjectPageData()
        : base(TYPE)
    {
    }
}
