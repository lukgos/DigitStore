using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Auth;
using Shared.Infrastructure.Auth;

namespace Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        
        return services;
    }
}