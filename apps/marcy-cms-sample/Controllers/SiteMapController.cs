using System.Text;
using CandyKingdom.Marcy.Immutables;
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
    public async Task<IActionResult> SitemapXml()
    {
        var urlBases = new List<string>()
        {
            $"",
            $"/about",
            $"/projects",
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            // var projects = await context.Projects
            //     .Where(x => x.Page.PublishStatus == PublishStatus.Published)
            //     .Select(x => new { x.Page.RouteName, })
            //     .ToListAsync();

            // var projectCategories = await context.ProjectCategories
            //     .Select(x => new { x.RouteName, })
            //     .ToListAsync();

            // projects.ForEach(x =>
            //     urlBases.Add($"/projects/{x.RouteName}")
            // );

            // projectCategories.ForEach(x =>
            //  urlBases.Add($"/areas/{x.RouteName}")
            // );
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
