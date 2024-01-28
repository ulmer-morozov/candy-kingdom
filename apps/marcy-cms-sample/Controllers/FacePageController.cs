using CandyKingdom.Marcy.Pages;

using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Sample.Controllers;

[ApiController]
public sealed class FacePageController : ControllerBase
{
    private readonly IPageManager _pageManager;
    private readonly ILogger<FacePageController> _logger;

    public FacePageController(IPageManager pageManager, ILogger<FacePageController> logger)
    {
        _pageManager = pageManager;
        _logger = logger;
    }

    [Route("Api/Pages/")]
    [HttpGet]
    public async Task<ActionResult<Page>> Get([FromQuery] string url = "", CancellationToken cancellationToken = default)
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
}
