using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    private static readonly Type PermissionCheckType = typeof(IPermissionCheck<>);
    
    public static void AddPermissionChecks(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var checkers = assembly
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == PermissionCheckType))
            .Select(x => new
                {
                    CheckerType = x,
                    PermissionCheckGenericType = x
                        .GetInterfaces()
                        .First(i => i.GetGenericTypeDefinition() == PermissionCheckType)
                        .GetGenericArguments()[0]
                });

        foreach (var checker in checkers)
        {
            serviceCollection.AddTransient(
                PermissionCheckType.MakeGenericType(checker.PermissionCheckGenericType),
                checker.CheckerType);
        }
    }
    
    public static void AddPermissionChecks(this IServiceCollection serviceCollection, Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            serviceCollection.AddPermissionChecks(assembly);
        }
    }
}