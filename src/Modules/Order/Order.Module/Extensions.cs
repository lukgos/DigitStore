using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Order.Module.DAL;
using Order.Module.DAL.Repositories;
using Order.Module.Features.CreateOrderFromProduct;
using Order.Module.Features.GetOrderById;
using Order.Module.Repositories;
using Shared.Application;
using Shared.EntityFramework;

namespace Order.Module;

public static class Extensions
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleServices(typeof(Extensions).Assembly);
        
        services.AddPostgres<OrderDbContext>(configuration);
        
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
    
    public static IEndpointRouteBuilder MapOrderModuleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateOrderFromProductEndpoint();
        app.MapGetOrderByIdEndpoint();

        return app;
    }
}