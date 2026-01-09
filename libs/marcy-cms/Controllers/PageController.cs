using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CandyKingdom.MarcyCms.Controllers;

[Route("Api/Pages")]
public sealed class PageController : ControllerBase
{
    private readonly IPageManager _pageManager;
    private readonly ILogger<PageController> _logger;

    public PageController(IPageManager pageManager, ILogger<PageController> logger)
    {
        _pageManager = pageManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<Results<NotFound<string>, Ok<Page>>> Get([FromQuery] string url = "", CancellationToken cancellationToken = default)
    {
        var pageResult = await _pageManager.GetAsync(
             url,
             new GetPageParams
             {
                 IncludeChildren = false,
                 PublishStatus = PublishStatus.Published
             },
             cancellationToken
         );

        if (!pageResult.IsSuccessful && (CRUDErrorCode)pageResult.ErrorCode == CRUDErrorCode.NotFound)
        {
            return TypedResults.NotFound("Page not found");
        }

        if (!pageResult.IsSuccessful)
        {
            _logger.LogError(pageResult);

            throw new Exception(pageResult.Message);
        }

        return TypedResults.Ok(pageResult.Data);
    }

    [HttpGet("Children")]
    public async Task<Results<NotFound<string>, Ok<ImmutableList2<Page>>>> GetChildren([FromQuery] string url = "", CancellationToken cancellationToken = default)
    {
        var getParams = new GetChildrenParams
        {
            PublishStatus = PublishStatus.Published
        };

        var childrenResult = await _pageManager.GetChildrenAsync(url, getParams, cancellationToken);

        if (!childrenResult.IsSuccessful && childrenResult.ErrorCode == (int)CRUDErrorCode.NotFound)
        {
            return TypedResults.NotFound($"Page with url = {url} hasn't been found");
        }

        if (!childrenResult.IsSuccessful)
        {
            _logger.LogError(childrenResult);

            throw new Exception(childrenResult.Message);
        }

        return TypedResults.Ok(childrenResult.Data);
    }
}
