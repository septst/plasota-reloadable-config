using Microsoft.Extensions.Logging;

namespace RuntimeLoggingChanges;

internal class TopLevelPeriodicallyLoggingService : PeriodicallyLoggingService
{
    public TopLevelPeriodicallyLoggingService(
        ILogger<TopLevelPeriodicallyLoggingService> logger)
        : base(logger)
    {
    }
}