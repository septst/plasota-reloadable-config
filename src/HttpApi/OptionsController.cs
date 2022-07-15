﻿using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HttpApi;

[ApiController]
[Route("configuration/ioptions")]
public class OptionsController : ControllerBase
{
    private readonly ILogger<OptionsController> _logger;
    private readonly ControllerOptions _options;

    public OptionsController(
        ILogger<OptionsController> logger,
        IOptions<ControllerOptions> options)
    {
        _logger = logger;
        _options = options.Value;
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
            ResolvedValue = _options.ConfiguredValue,
        });
    }
}