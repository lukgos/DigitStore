using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.CQRS;

namespace Shared.Application;

public static class Extensions
{
    public static IServiceCollection AddModuleServices(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(s => s
            .FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
        );

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}