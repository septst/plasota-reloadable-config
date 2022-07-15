using System;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using HttpApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
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
            var keyVaultName = builder.Configuration.GetValue<string?>("KeyVaultName");

            if (string.IsNullOrWhiteSpace(keyVaultName))
            {
                startupLogger.Information("No Key Vault specified");
            }
            else
            {
                startupLogger.Information("Specified \"{KeyVaultName}\" Key Vault", keyVaultName);
                try
                {
                    builder.Configuration.AddAzureKeyVault(
                        new Uri($"https://{keyVaultName}.vault.azure.net/"),
                        new DefaultAzureCredential(),
                        new AzureKeyVaultConfigurationOptions
                        {
                            ReloadInterval = TimeSpan.FromMinutes(1),
                        });
                }
                catch (Exception ex)
                {
                    startupLogger.Error(ex, "Failed adding Azure Key Vault");
                }
            }

            builder.Host.UseSerilog(
                (ctx, cfg) =>
                {
                    cfg.ReadFrom.Configuration(ctx.Configuration);
                });

            builder.Services
               .AddOptions<ControllerOptions>()
               .Bind(builder.Configuration.GetSection(ControllerOptions.Section))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddLoggingServicesInDifferentNestedNamespaces();

            var application = builder.Build();

            application.MapHealthChecks("health/live", new HealthCheckOptions { Predicate = _ => false });
            application.MapHealthChecks("health/ready");
            application.MapControllers();

            application.Run();
        }
        catch (Exception exception)
        {
            startupLogger.Fatal(exception, "Startup failed");
        }
    }
}