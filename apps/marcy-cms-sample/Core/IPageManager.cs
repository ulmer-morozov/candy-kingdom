using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms.Sample.Core;

interface IPageManager
{
    Task<ResultOrError<Page>> Get(string routeName, GetPageParams? parameters= null);
    Task<ResultOrError> Store(Page page, bool ignoreBones = false);
}
