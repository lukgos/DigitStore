using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;

namespace Search.Module.Infrastructure;

internal static class OpenSearchExtensions
{
    public static IServiceCollection AddOpenSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new OpenSearchOptions();
        configuration.GetSection("OpenSearch").Bind(options);

        var settings = new ConnectionSettings(new Uri(options.Url))
            .DefaultIndex("products")
            .EnableDebugMode();

        var client = new OpenSearchClient(settings);
        
        services.AddSingleton<IOpenSearchClient>(client);
        
        //services.AddHostedService<OpenSearchInitializer>();

        return services;
    }
}