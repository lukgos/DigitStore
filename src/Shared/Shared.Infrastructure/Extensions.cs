using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Auth;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Messaging;

namespace Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,  IConfiguration configuration, params Assembly[] moduleAssemblies)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        services.AddMessaging(configuration, moduleAssemblies);
        
        return services;
    }
}