using CandyKingdom.Marcy;

using Microsoft.Extensions.Logging;

namespace CandyKingdom.MarcyCms;

public static class LoggerExtensions
{
    public static void LogError(this ILogger logger, ResultOrError result)
    {
        var errorCodeText = result.ErrorCode == 0 ? "" : $"[{result.ErrorCode}] ";
        logger.LogError($"{errorCodeText}{result.Message}");
    }
}
