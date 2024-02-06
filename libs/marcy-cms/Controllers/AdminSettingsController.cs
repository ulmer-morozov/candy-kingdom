using CandyKingdom.Marcy.Immutables;
using CandyKingdom.MarcyCms.Settings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CandyKingdom.MarcyCms.Sample.Controllers.Admin;

[Authorize]
[Route("Api/Admin/Settings")]
public sealed class AdminSettingsController : ControllerBase
{
    private readonly ISettingsManager _settingsManager;
    private readonly ILogger<AdminSettingsController> _logger;

    public AdminSettingsController(ISettingsManager settingsManager, ILogger<AdminSettingsController> logger)
    {
        _settingsManager = settingsManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<Ok<ImmutableList2<SettingGroup>>> GetAll(CancellationToken cancellationToken = default)
    {
        var groupsResult = await _settingsManager.GetGroupsAsync(cancellationToken: cancellationToken);

        if (!groupsResult.IsSuccessful)
        {
            _logger.LogError(groupsResult);

            throw new Exception(groupsResult.Message);
        }

        return TypedResults.Ok(groupsResult.Data);
    }

    [HttpPost]
    public async Task<Ok> Store([FromBody] IEnumerable<Setting> settings, CancellationToken cancellationToken = default)
    {
        var updateResult = await _settingsManager.UpdateAsync(settings, cancellationToken);

        if (!updateResult.IsSuccessful)
        {
            _logger.LogError(updateResult);

            throw new Exception(updateResult.Message);
        }

        return TypedResults.Ok();
    }
}
