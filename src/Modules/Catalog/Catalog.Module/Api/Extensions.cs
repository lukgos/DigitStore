using Catalog.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Module.Api;

internal static class Extensions
{
    public static IServiceCollection AddCatalogApi(this IServiceCollection services)
    {
        services.AddScoped<ICatalogApi, CatalogApi>();
    
        return services;
    }
}