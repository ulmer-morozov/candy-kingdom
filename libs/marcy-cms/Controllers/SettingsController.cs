using System.Collections.Immutable;

using CandyKingdom.MarcyCms.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CandyKingdom.MarcyCms.Controllers;

[Route("Api/Settings")]
public sealed class SettingsController : ControllerBase
{
    private readonly ISettingsManager _settingsManager;
    private readonly ILogger<AdminSettingsController> _logger;

    public SettingsController(ISettingsManager settingsManager, ILogger<AdminSettingsController> logger)
    {
        _settingsManager = settingsManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<Results<UnprocessableEntity<string>, Ok<ImmutableDictionary<Guid, Setting>>>> Get([FromQuery] Guid[] ids, CancellationToken cancellationToken = default)
    {
        var settingResult = await _settingsManager.GetSettingsAsync(ids, cancellationToken);

        if (!settingResult.IsSuccessful && settingResult.ErrorCode == (int)CRUDErrorCode.NotFound)
        {
            return TypedResults.UnprocessableEntity(settingResult.Message);
        }

        if (!settingResult.IsSuccessful)
        {
            _logger.LogError(settingResult);

            throw new Exception(settingResult.Message);
        }

        var dict = settingResult.Data.ToImmutableDictionary(x => x.Id);

        return TypedResults.Ok(dict);
    }
}
