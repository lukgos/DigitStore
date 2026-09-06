using Account.Module.DAL;
using Catalog.Module.DAL;
using Microsoft.EntityFrameworkCore;
using Order.Module.DAL;

namespace DigitStore.Api;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var accountDbContext = services.GetRequiredService<AccountDbContext>();
            var catalogDbContext = services.GetRequiredService<CatalogDbContext>();
            var orderDbContext = services.GetRequiredService<OrderDbContext>();

            await accountDbContext.Database.MigrateAsync();
            await catalogDbContext.Database.MigrateAsync();
            await orderDbContext.Database.MigrateAsync();
            
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error occurred while migrating database.");
            throw;
        }
    }
}