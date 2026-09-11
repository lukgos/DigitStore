using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Messaging.Options;

namespace Shared.Infrastructure.Messaging;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration, params Assembly[] moduleAssemblies)
    {
        var options = new RabbitOptions();
        configuration.GetSection("RabbitMQ").Bind(options);

        services.AddMassTransit(x =>
        {
            if (moduleAssemblies.Any())
            {
                x.AddConsumers(moduleAssemblies);
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(options.Host, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}