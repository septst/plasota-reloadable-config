using Microsoft.Extensions.Logging;

namespace RuntimeLoggingChanges.Nested.SubNested;

internal class SubNestedPeriodicallyLoggingService : PeriodicallyLoggingService
{
    public SubNestedPeriodicallyLoggingService(
        ILogger<SubNestedPeriodicallyLoggingService> logger)
        : base(logger)
    {
    }
}