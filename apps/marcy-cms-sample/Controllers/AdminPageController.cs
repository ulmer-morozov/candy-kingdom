using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Sample.Controllers.Admin;

[Authorize(Policy = Roles.Admin)]
[Route("Api/Admin/Page")]
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
        var getParams = new GetPageParams
        {
            IncludeChildren = false,
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

        return pageResult.Data;
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
