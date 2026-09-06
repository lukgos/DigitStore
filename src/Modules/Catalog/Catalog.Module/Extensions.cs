using Catalog.Module.Api;
using Catalog.Module.DAL;
using Catalog.Module.DAL.Repositories;
using Catalog.Module.Features.AddCategory;
using Catalog.Module.Features.AddProduct;
using Catalog.Module.Features.DeleteCategory;
using Catalog.Module.Features.GetProduct;
using Catalog.Module.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared.Application;

namespace Catalog.Module;

public static class Extensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleServices(typeof(Extensions).Assembly);

        services.AddCatalogApi();
        
        var connectionString = configuration.GetConnectionString("Postgres");
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson(); 
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });
        
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
    
    public static IEndpointRouteBuilder MapCatalogModuleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAddProductEndpoint();
        app.MapGetProductEndpoint();
        
        app.MapAddCategoryEndpoint();
        app.MapDeleteCategoryEndpoint();

        return app;
    }
}