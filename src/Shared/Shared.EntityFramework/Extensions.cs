using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared.EntityFramework.Interceptors;

namespace Shared.EntityFramework;

public static class Extensions
{
    public static IServiceCollection AddEntityFrameworkServices(this IServiceCollection services)
    {
        services.AddScoped<AuditableEntityInterceptor>();

        return services;
    }
    
    public static IServiceCollection AddPostgres<TDbContext>(this IServiceCollection services, IConfiguration configuration,
        Action<NpgsqlDataSourceBuilder>? configureDataSource = null) where TDbContext : DbContext
    {
        services.AddScoped<AuditableEntityInterceptor>();
    
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            
            var connectionString = configuration.GetConnectionString("Postgres");
                
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            configureDataSource?.Invoke(dataSourceBuilder);
            
            var dataSource = dataSourceBuilder.Build();

            options.UseNpgsql(dataSource).AddInterceptors(interceptor);
        });
        
        return services;
    }
}