using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Saturn.Domain.Services;
using Saturn.Infrastructure.Database;
using Saturn.Infrastructure.Services;

namespace Saturn.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddMemoryCache()
            .AddTransient<IMessageService, MessageService>();

    public static IServiceCollection AddSaturnContext(this IServiceCollection serviceCollection, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return serviceCollection.AddDbContextFactory<SaturnContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, ServiceLifetime.Transient);
    }
}