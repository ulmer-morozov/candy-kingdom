using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Data;

using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms;

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
        url = url.ToLowerInvariant();
        parameters ??= new GetPageParams();

        PageDb? pageDb;

        await using (var context = await _contextFactory.CreateDbContextAsync(cancellationToken))
        {
            IQueryable<PageDb> pageQuery = context.Pages;

            if (parameters.IncludeChildren)
            {
                pageQuery = pageQuery.Include(x => x.Children);
            }

            if (parameters.PublishStatus != PublishStatus.NotSet)
            {
                pageQuery = pageQuery.Where(x => x.PublishStatus == PublishStatus.Published);
            }

            pageDb = await pageQuery
                                   .Where(x => x.Url == url)
                                   .SingleOrDefaultAsync(cancellationToken);
        }

        if (pageDb == null)
        {
            return ResultOrError.Fail<Page>($"Page with Url {url} not found.", (int)CRUDErrorCode.NotFound);
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
            return ResultOrError.Fail($"Page with Url = {page.Url} not found.", (int)CRUDErrorCode.NotFound);
        }

        pageDb.Copy(page);

        await context.SaveChangesAsync(cancellationToken);

        return ResultOrError.Success();
    }

    private static Page ToDto(PageDb pageDb)
    {
        var children = (pageDb.Children ?? Array.Empty<PageDb>())
                                            .Select(ToDto)
                                            .OrderBy(x => x.Order)
                                            .ToImmutableList2();

        var page = new Page()
        {
            Id = pageDb.Id,
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

    public async Task<ResultOrError<ImmutableList2<Page>>> GetChildrenAsync(string url, GetChildrenParams? parameters = null, CancellationToken cancellationToken = default)
    {
        url = url == "~" ? url : url.Replace("~", "").ToLowerInvariant();

        parameters ??= new GetChildrenParams();

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        IQueryable<PageDb> pageQuery = context.Pages.Include(x => x.Children);

        if (parameters.PublishStatus != PublishStatus.NotSet)
        {
            pageQuery = pageQuery.Where(x => x.PublishStatus == PublishStatus.Published);
        }

        var mainPage = await pageQuery.SingleOrDefaultAsync
            (
                x => x.Url == url, cancellationToken
            );

        if (mainPage == null)
        {
            return ResultOrError.Fail<ImmutableList2<Page>>($"Page with url = {url} hasn't been found", (int)CRUDErrorCode.NotFound);
        }

        var children = mainPage.Children
            .Select(x => new Page
            {
                Id = x.Id,
                Title = x.Title,
                Url = x.Url,
                Order = x.Order
            })
            .OrderBy(x => x.Order)
            .ToImmutableList2();

        return ResultOrError.Success(children);
    }
}
