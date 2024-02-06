using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;

namespace CandyKingdom.MarcyCms;

public interface IViewManager
{
    Task<ResultOrError<View>> GetAsync(string code, CancellationToken cancellationToken = default);
}
