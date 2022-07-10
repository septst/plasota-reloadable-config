using Microsoft.Extensions.Logging;

namespace RuntimeLoggingChanges.Nested;

internal class NestedPeriodicallyLoggingService : PeriodicallyLoggingService
{
    public NestedPeriodicallyLoggingService(
        ILogger<NestedPeriodicallyLoggingService> logger)
        : base(logger)
    {
    }
}