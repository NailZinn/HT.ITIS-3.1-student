using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] handlersAssemblies)
    {
        var iRequestHandler = typeof(IRequestHandler<,>);

        var types = handlersAssemblies
            .SelectMany(x => x.GetTypes()
                .Where(t => t is { IsAbstract: false, IsClass: true }))
            .ToArray();

        var interfaces = types
            .SelectMany(x => x.GetInterfaces())
            .Where(x => x.IsGenericType)
            .Where(x => x.GetGenericTypeDefinition() == iRequestHandler);

        foreach (var @interface in interfaces)
        {
            var implementations = types.Where(x => x.IsAssignableTo(@interface));

            foreach (var implementation in implementations)
            {
                services.AddTransient(@interface, implementation);
            }
        }
        
        services.AddTransient<IMediator, Mediator>();

        return services;
    }
}