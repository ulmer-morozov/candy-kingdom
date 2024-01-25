using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Content;

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
    public async Task<ActionResult<Page>> Get([FromQuery] string url = "", CancellationToken cancellationToken = default)
    {
        url = url.Replace("~", "").ToLowerInvariant();

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var mainPage = await context.Pages
        .Include(x => x.Children)
        .SingleOrDefaultAsync
        (
            x => x.Url == url, cancellationToken
        );

        return new Page();
    }
}
