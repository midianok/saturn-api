using Microsoft.EntityFrameworkCore;
using Saturn.Infrastructure.Database;

namespace Saturn.Api.Extensions;

public static class Extensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<SaturnContext>>()
            .CreateDbContext();
       
        dbContext.Database.Migrate();
    }
}