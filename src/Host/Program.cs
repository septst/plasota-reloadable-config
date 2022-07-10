using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using RuntimeLoggingChanges;
using Serilog;

namespace Host;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var startupLogger = new LoggerConfiguration()
           .ReadFrom.Configuration(builder.Configuration)
           .CreateLogger();

        try
        {
            builder.Host.UseSerilog(
                (ctx, cfg) =>
                {
                    cfg.ReadFrom.Configuration(ctx.Configuration);
                });

            builder.Services.AddLoggingServicesInDifferentNestedNamespaces();
            builder.Services.AddHealthChecks();

            var application = builder.Build();

            application.MapHealthChecks("health/live", new HealthCheckOptions
            {
                Predicate = _ => false,
            });
            application.MapHealthChecks("health/ready");
            application.Run();
        }
        catch (Exception exception)
        {
            startupLogger.Fatal(exception, "Startup failed");
        }
    }
}