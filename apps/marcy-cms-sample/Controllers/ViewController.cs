using System.Collections.Immutable;

using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Content;
using CandyKingdom.MarcyCms.Sample.Core;

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
    public async Task<ActionResult<IEnumerable<Page>>> Get([FromQuery] string url = "", CancellationToken cancellationToken = default)
    {
        url = url.Replace("~", "").ToLowerInvariant();

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var mainPage = await context.Pages
            .Include(x => x.Children)
            .Where(x => x.PublishStatus == PublishStatus.Published)
            .SingleOrDefaultAsync
            (
                x => x.Url == url, cancellationToken
            );

        var children = (mainPage?.Children ?? [])
            .Select(x => new Page
            {
                Id = x.Id,
                Title = x.Title,
                Url = x.Url
            })
            .ToImmutableList();

        return Ok(children);
    }
}
