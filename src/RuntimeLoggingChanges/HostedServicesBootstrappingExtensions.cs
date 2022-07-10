using Microsoft.Extensions.DependencyInjection;
using RuntimeLoggingChanges.Nested;
using RuntimeLoggingChanges.Nested.SubNested;

namespace RuntimeLoggingChanges;

public static class HostedServicesBootstrappingExtensions
{
    public static void AddLoggingServicesInDifferentNestedNamespaces(this IServiceCollection services)
    {
        services.AddHostedService<TopLevelPeriodicallyLoggingService>();
        services.AddHostedService<NestedPeriodicallyLoggingService>();
        services.AddHostedService<SubNestedPeriodicallyLoggingService>();
    }
}