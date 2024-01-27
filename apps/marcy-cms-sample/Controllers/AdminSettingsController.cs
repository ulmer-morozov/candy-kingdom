using CandyKingdom.Marcy.Immutables;
using CandyKingdom.MarcyCms.Settings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyKingdom.MarcyCms.Sample.Controllers.Admin;

[Authorize]
[Route("Api/Admin/Settings")]
public class AdminSettingsController : Controller
{
    private readonly ISettingsManager _settingsManager;
    private readonly ILogger<AdminSettingsController> _logger;

    public AdminSettingsController(ISettingsManager settingsManager, ILogger<AdminSettingsController> logger)
    {
        _settingsManager = settingsManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ImmutableList2<SettingGroup>>> GetAll(CancellationToken cancellationToken = default)
    {
        var groupsResult = await _settingsManager.GetGroupsAsync(cancellationToken: cancellationToken);

        if (!groupsResult.IsSuccessful)
        {
            _logger.LogError(groupsResult);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return groupsResult.Data;
    }

    [Route("SiteData")]
    [HttpPost]
    public async Task<IActionResult> Store([FromBody] IEnumerable<Setting> settings, CancellationToken cancellationToken = default)
    {
        var updateResult = await _settingsManager.UpdateAsync(settings, cancellationToken);

        if (!updateResult.IsSuccessful)
        {
            _logger.LogError(updateResult);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok();
    }
}
