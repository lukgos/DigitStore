using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Search.Module.Features.SearchProducts;
using Search.Module.Infrastructure;
using Shared.Application;

namespace Search.Module;

public static class Extensions
{
    public static IServiceCollection AddSearchModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleServices(typeof(Extensions).Assembly);
        services.AddOpenSearch(configuration);
        
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapSearchModuleEndpoints(this IEndpointRouteBuilder app)
    {
       app.MapSearchProductsEndpoint();

        return app;
    }
}