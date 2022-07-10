using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RuntimeLoggingChanges;

internal abstract class PeriodicallyLoggingService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));

    protected PeriodicallyLoggingService(
        ILogger logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) &&
               !stoppingToken.IsCancellationRequested)
        {
            _logger.LogOnMultipleLevels(GetType().Name);
        }
    }
}