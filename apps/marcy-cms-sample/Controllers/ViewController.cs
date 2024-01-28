using System.Collections.Immutable;

using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Content;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Sample.Controllers;

[ApiController]
public sealed class ViewController : ControllerBase
{
    private readonly IDbContextFactory<CmsSampleDbContext> _contextFactory;
    private readonly ILogger<ViewController> _logger;

    public ViewController(IDbContextFactory<CmsSampleDbContext> contextFactory, ILogger<ViewController> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    [Route("Api/Children/")]
    [HttpGet]
    public async Task<Results<NotFound<string>, Ok<ImmutableList<Page>>>> Get([FromQuery] string url = "", CancellationToken cancellationToken = default)
    {
        url = url == "~" ? url : url.Replace("~", "").ToLowerInvariant();

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var mainPage = await context.Pages
            .Include(x => x.Children)
            .Where(x => x.PublishStatus == PublishStatus.Published)
            .SingleOrDefaultAsync
            (
                x => x.Url == url, cancellationToken
            );

        if (mainPage == null)
        {
            return TypedResults.NotFound($"Page with url = {url} hasn't been found");
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
            .ToImmutableList();

        return TypedResults.Ok(children);
    }

    [Route("Api/Views/{code}")]
    [HttpGet]
    public async Task<Results<NotFound<string>, UnprocessableEntity<string>, Ok<View>>> GetView(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return TypedResults.UnprocessableEntity($"code cannot be empty or whitespace");
        }

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var viewDb = await context.Views
            .SingleOrDefaultAsync
            (
                x => x.Code == code,
                cancellationToken
            );

        if (viewDb == null)
        {
            return TypedResults.NotFound($"View with code = {code} hasn't been found");
        }

        var view = new View
        {
            Id = viewDb.Id,
            Code = viewDb.Code,
            Bones = viewDb.Bones
        };

        return TypedResults.Ok(view);
    }
}
