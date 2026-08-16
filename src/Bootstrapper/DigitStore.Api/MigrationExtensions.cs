using Account.Module.DAL;
using Microsoft.EntityFrameworkCore;

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
            
            await accountDbContext.Database.MigrateAsync();
            
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error occurred while migrating database.");
            throw;
        }
    }
}