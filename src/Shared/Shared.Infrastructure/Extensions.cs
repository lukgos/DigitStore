using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Shared.Abstractions.Auth;
using Shared.Abstractions.CQRS;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Decorators;
using Shared.Infrastructure.Messaging;
using Shared.Infrastructure.Middlewares;

namespace Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,  IConfiguration configuration, params Assembly[] moduleAssemblies)
    {
        services.AddScoped<ExceptionMiddleware>();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => c.CustomSchemaIds(x => x.FullName));
        
        services.AddSerilog((serviceProvider, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(serviceProvider)
            .Enrich.FromLogContext());
        
        var serviceName = configuration.GetValue<string>("Telemetry:ServiceName");
        var otlpEndpoint = configuration.GetValue<string>("Telemetry:Endpoint");

        services.AddOpenTelemetry()
            .ConfigureResource(res => res.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddNpgsql()
                .AddOtlpExporter(opt => opt.Endpoint = new Uri(otlpEndpoint)))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter());
        
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        services.AddMessaging(configuration, moduleAssemblies);
        
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
        
        return services;
    }
    
    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseSerilogRequestLogging();
        app.UseMiddleware<ExceptionMiddleware>();
        
        app.UseAuthentication();
        app.UseAuthorization();
    
        return app;
    }
    
    public static IEndpointRouteBuilder MapInfrastructureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPrometheusScrapingEndpoint()
            .AddEndpointFilter(async (context, next) =>
            {
                var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var expectedToken = config.GetValue<string>("Telemetry:MetricsToken");
                var authHeader = context.HttpContext.Request.Headers.Authorization.ToString();
                
                if (authHeader != $"Bearer {expectedToken}")
                {
                    return Results.Unauthorized();
                }
                
                return await next(context);
            });

        return endpoints;
    }
}