using System.Web;

using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Sample.Controllers.Admin;

[Authorize]
[Route("Api/Admin/Pages")]
public class AdminPageController : ControllerBase
{
    private readonly IPageManager _pageManager;
    private readonly ILogger<AdminPageController> _logger;

    public AdminPageController(IPageManager pageManager, ILogger<AdminPageController> logger)
    {
        _pageManager = pageManager;
        _logger = logger;
    }

    [Route("{url}")]
    [HttpGet]
    public async Task<ActionResult<Page>> Get(string url, CancellationToken cancellationToken = default)
    {
        url = url == "~" ? url : HttpUtility.UrlDecode(url).Substring(1);
        var getParams = new GetPageParams
        {
            IncludeChildren = true,
        };

        var pageResult = await _pageManager.GetAsync(url, getParams, cancellationToken);

        if (!pageResult.IsSuccessful && (CRUDPageErrorCode)pageResult.ErrorCode == CRUDPageErrorCode.NotFound)
        {
            return NotFound("Page not found");
        }

        if (!pageResult.IsSuccessful)
        {
            _logger.LogError(pageResult);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var resultPage = pageResult.Data with
        {
            Children = pageResult.Data.Children
                .Select(x => new Page { Id = x.Id, Url = x.Url, Title = x.Title })
                .ToImmutableList2()
        };

        return resultPage;
    }

    [HttpPost]
    public async Task<IActionResult> StorePage([FromBody] Page page, CancellationToken cancellationToken = default)
    {
        var pageResult = await _pageManager.StoreAsync(page, cancellationToken);

        if (!pageResult.IsSuccessful && (CRUDPageErrorCode)pageResult.ErrorCode == CRUDPageErrorCode.NotFound)
        {
            return NotFound("Page not found");
        }

        if (!pageResult.IsSuccessful)
        {
            _logger.LogError(pageResult);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok();
    }
}
