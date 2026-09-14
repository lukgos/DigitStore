using Account.Module.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Account.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<AccountTestApp>, IAsyncLifetime
{
    protected readonly AccountTestApp Factory;
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(AccountTestApp factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
    }

    public virtual ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public virtual async ValueTask DisposeAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
        
        await dbContext.Users.ExecuteDeleteAsync();
    }
}