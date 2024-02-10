using System.Web;

using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CandyKingdom.MarcyCms.Controllers;

[Authorize]
[Route("Api/Admin/Pages")]
public sealed class AdminPageController : ControllerBase
{
    private readonly IPageManager _pageManager;
    private readonly ILogger<AdminPageController> _logger;

    public AdminPageController(IPageManager pageManager, ILogger<AdminPageController> logger)
    {
        _pageManager = pageManager;
        _logger = logger;
    }

    [HttpGet("{url}")]
    public async Task<Results<NotFound<string>, Ok<Page>>> Get(string url, CancellationToken cancellationToken = default)
    {
        url = url == "~" ? url : HttpUtility.UrlDecode(url).Substring(1);

        var getParams = new GetPageParams
        {
            IncludeChildren = true,
        };

        var pageResult = await _pageManager.GetAsync(url, getParams, cancellationToken);

        if (!pageResult.IsSuccessful && pageResult.ErrorCode == (int)CRUDErrorCode.NotFound)
        {
            return TypedResults.NotFound($"Page with url = {url} hasn't been found"); // todo: make JSON error responses
        }

        if (!pageResult.IsSuccessful)
        {
            _logger.LogError(pageResult);

            throw new Exception(pageResult.Message);
        }

        var resultPage = pageResult.Data with
        {
            Children = pageResult.Data.Children
                .Select(x => new Page { Id = x.Id, Url = x.Url, Title = x.Title })
                .ToImmutableList2()
        };

        return TypedResults.Ok(resultPage);
    }

    [HttpPost]
    public async Task<Results<NotFound<string>, Ok>> StorePage([FromBody] Page page, CancellationToken cancellationToken = default)
    {
        var pageResult = await _pageManager.StoreAsync(page, cancellationToken);

        if (!pageResult.IsSuccessful && pageResult.ErrorCode == (int)CRUDErrorCode.NotFound)
        {
            return TypedResults.NotFound($"Page with url = {page.Url} hasn't been found"); // todo: make JSON error responses
        }

        if (!pageResult.IsSuccessful)
        {
            _logger.LogError(pageResult);

            throw new Exception(pageResult.Message);
        }

        return TypedResults.Ok();
    }
}
