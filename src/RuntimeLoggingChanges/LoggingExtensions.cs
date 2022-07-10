using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace RuntimeLoggingChanges;

internal static class LoggingExtensions
{
    public static void LogOnMultipleLevels(this ILogger logger, string serviceName, LogLevel maxLevel = LogLevel.Information)
    {
        foreach (var logLevel in Enum
                    .GetValues<LogLevel>()
                    .Where(logLevel => logLevel <= maxLevel))
        {
            logger.Log(
                logLevel,
                "{Service} logging as {Level} at {Timestamp}",
                serviceName,
                logLevel,
                DateTimeOffset.Now);
        }
    }
}