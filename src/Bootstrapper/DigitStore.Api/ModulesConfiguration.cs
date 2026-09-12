using System.Reflection;
using Account.Module;
using Catalog.Module;
using Order.Module;
using Search.Module;

namespace DigitStore.Api;

internal static class ModulesConfiguration
{
    public static readonly Assembly[] Assemblies = 
    [
        typeof(Catalog.Module.Extensions).Assembly,
        typeof(Search.Module.Extensions).Assembly,
        typeof(Account.Module.Extensions).Assembly,
        typeof(Customer.Module.Extensions).Assembly,
        typeof(Payment.Module.Extensions).Assembly,
        typeof(Order.Module.Extensions).Assembly,
        typeof(Distribution.Module.Extensions).Assembly
    ];

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAccountModule(configuration);
        services.AddCatalogModule(configuration);
        services.AddSearchModule(configuration);
        services.AddOrderModule(configuration);
        
        return services;
    }

    public static IEndpointRouteBuilder MapModuleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAccountModuleEndpoints();
        app.MapCatalogModuleEndpoints();
        app.MapSearchModuleEndpoints();
        app.MapOrderModuleEndpoints();
        
        return app;
    }
}