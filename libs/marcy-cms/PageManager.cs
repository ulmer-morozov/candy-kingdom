using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Data;

using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Sample.Core;

public class PageManager<TDbContext> : IPageManager
    where TDbContext : DbContext, IMarcyCmsDbContext
{
    private readonly IDbContextFactory<TDbContext> _contextFactory;

    public PageManager(IDbContextFactory<TDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // For generated pages we could override this method
    public virtual async Task<ResultOrError<Page>> GetAsync(string url, GetPageParams? parameters = null, CancellationToken cancellationToken = default)
    {
        parameters ??= new GetPageParams();

        PageDb? pageDb;

        await using (var context = await _contextFactory.CreateDbContextAsync(cancellationToken))
        {
            IQueryable<PageDb> pageQuery = context.Pages;

            if (parameters.IncludeChildren)
            {
                pageQuery = pageQuery.Include(x => x.Parent);
            }

            if (parameters.publishStatus != PublishStatus.NotSet)
            {
                pageQuery = pageQuery.Where(x => x.PublishStatus == PublishStatus.Published);
            }

            pageDb = await pageQuery
                                   .Where(x => x.Url.Equals(url, StringComparison.OrdinalIgnoreCase))
                                   .SingleOrDefaultAsync(cancellationToken);
        }

        if (pageDb == null)
        {
            return ResultOrError.Fail<Page>($"Page with Url {url} not found.", (int)CRUDPageErrorCode.NotFound);
        }

        var dto = ToDto(pageDb);

        return ResultOrError.Success(dto);
    }

    public async Task<ResultOrError> StoreAsync(Page page, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var pageDb = await context.Pages.SingleOrDefaultAsync(x => x.Url == page.Url, cancellationToken);

        if (pageDb == null)
        {
            return ResultOrError.Fail($"Page with Url = {page.Url} not found.", (int)CRUDPageErrorCode.NotFound);
        }

        pageDb.Copy(page);

        await context.SaveChangesAsync(cancellationToken);

        return ResultOrError.Success();
    }

    private static Page ToDto(PageDb pageDb)
    {
        var children = (pageDb.Childern ?? Array.Empty<PageDb>())
                                            .Select(ToDto)
                                            .ToImmutableList2();

        var page = new Page()
        {
            Url = pageDb.Url,
            Route = pageDb.Route,
            Order = pageDb.Order,
            PublishStatus = pageDb.PublishStatus,
            Title = pageDb.Title,
            OpenGraph = pageDb.OpenGraph,
            Bones = pageDb.Bones,
            Children = children
        };

        return page;
    }
}
