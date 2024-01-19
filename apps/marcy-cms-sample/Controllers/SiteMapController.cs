using System.Text;

using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Content;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Sample.Controllers;

public sealed class SiteMapController : Controller
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public SiteMapController(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    [HttpGet("/sitemap.xml")]
    public async Task<IActionResult> SitemapXml(CancellationToken cancellationToken = default)
    {
        List<string> urlBases = [];

        using (var context = await _contextFactory.CreateDbContextAsync(cancellationToken))
        {
            var pageUrls = await context.Pages
                .Where(x => x.PublishStatus == PublishStatus.Published)
                .Select(x => x.Url)
                .ToListAsync(cancellationToken);

            urlBases.AddRange(pageUrls);
        }

        var host = Request.Scheme + "://" + Request.Host;

        var mainLanguage = "en";
        var altLanguages = new string[] { "ru" };

        var items = urlBases
        .Select
        (
            url => new SitemapItem
            {
                Loc = $"{host}/${mainLanguage}{url}",
                ChangeFreq = "monthly",
                Priority = "0.8",
                Alternates = altLanguages
                                   .Select(lang => new SiteMapItemAlternate
                                   {
                                       Href = $"{host}/{lang}{url}",
                                       HrefLang = lang,
                                   })
                                   .ToImmutableList2()
            }
        );

        var encoding = Encoding.UTF8;
        var xmlText = SitemapBuilder.BuildXml(items, encoding);

        return Content(xmlText, "application/xml", encoding);
    }
}
