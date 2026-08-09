using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Module;

public static class Extensions
{
    public static IServiceCollection AddAccount(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}