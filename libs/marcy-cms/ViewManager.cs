using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;

using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms;

public sealed class ViewManager<TDbContext> : IViewManager
    where TDbContext : DbContext, IMarcyCmsDbContext
{
    private readonly IDbContextFactory<TDbContext> _contextFactory;

    public ViewManager(IDbContextFactory<TDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<ResultOrError<View>> GetAsync(string code, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var viewDb = await context.Views
                   .SingleOrDefaultAsync
                   (
                       x => x.Code == code,
                       cancellationToken
                   );

        if (viewDb == null)
        {
            return ResultOrError.Fail<View>($"View with code = {code} hasn't been found", (int)CRUDPageErrorCode.NotFound);
        }

        var view = new View
        {
            Id = viewDb.Id,
            Code = viewDb.Code,
            Bones = viewDb.Bones
        };

        return ResultOrError.Success(view);
    }
}
