using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HttpApi;

[ApiController]
[Route("configuration/ioptionsmonitor")]
public class OptionsMonitorController : ControllerBase
{
    private readonly ILogger<OptionsMonitorController> _logger;
    private readonly IOptionsMonitor<ControllerOptions> _optionsMonitor;

    private ControllerOptions Options => _optionsMonitor.CurrentValue;

    public OptionsMonitorController(
        ILogger<OptionsMonitorController> logger,
        IOptionsMonitor<ControllerOptions> optionsMonitor)
    {
        _logger = logger;
        _optionsMonitor = optionsMonitor;
    }

    [HttpGet]
    public IActionResult GetConfiguredValue()
    {
        var receivedAt = DateTimeOffset.Now;
        _logger.LogDebug("Invoked a GET request at {TimeStamp}", DateTimeOffset.Now);

        _logger.LogDebug("About to perform some time-consuming computation");
        var sw = Stopwatch.StartNew();
        sw.Stop();
        _logger.LogDebug(
            "About to perform some time-consuming computation - Finished in {ElapsedMs}",
            sw.ElapsedMilliseconds);

        _logger.LogInformation("GET request handled");

        return Ok(new
        {
            ReceivedAt = receivedAt,
            CompletedAt = DateTimeOffset.Now,
            ResolvedValue = Options.ConfiguredValue,
        });
    }
}