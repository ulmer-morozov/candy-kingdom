using CandyKingdom.Marcy.Pages;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CandyKingdom.MarcyCms.Controllers;

[Route("Api/Views")]
public sealed class ViewController : ControllerBase
{
    private readonly IViewManager _viewManager;
    private readonly ILogger<ViewController> _logger;

    public ViewController(ILogger<ViewController> logger, IViewManager viewManager)
    {
        _logger = logger;
        _viewManager = viewManager;
    }

    [HttpGet("{code}")]
    public async Task<Results<NotFound<string>, UnprocessableEntity<string>, Ok<View>>> GetView(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return TypedResults.UnprocessableEntity($"code cannot be empty or whitespace");
        }

        var viewResult = await _viewManager.GetAsync(code, cancellationToken);


        if (!viewResult.IsSuccessful && viewResult.ErrorCode == (int)CRUDPageErrorCode.NotFound)
        {
            return TypedResults.NotFound($"View with code = {code} hasn't been found");
        }

        if (!viewResult.IsSuccessful)
        {
            _logger.LogError(viewResult);

            throw new Exception(viewResult.Message);
        }

        return TypedResults.Ok(viewResult.Data);
    }
}
