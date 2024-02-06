using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms;

public interface IPageManager
{
    Task<ResultOrError<Page>> GetAsync(string url, GetPageParams? parameters = null, CancellationToken cancellationToken = default);
    Task<ResultOrError> StoreAsync(Page page, CancellationToken cancellationToken = default);
    Task<ResultOrError<ImmutableList2<Page>>> GetChildrenAsync(string url, GetChildrenParams? parameters = null, CancellationToken cancellationToken = default);
}
