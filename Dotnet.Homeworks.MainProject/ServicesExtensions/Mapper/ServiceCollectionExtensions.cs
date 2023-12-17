using System.Reflection;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Mapper;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services, Assembly mapperConfigsAssembly)
    {
        var mappers = mapperConfigsAssembly
            .GetTypes()
            .Where(x => x is { IsClass: true, IsAbstract: false, IsGenericType: false } && x.Name.EndsWith("Mapper"))
            .Select(x => new
            {
                Interface = x.GetInterfaces().FirstOrDefault(i => i.Name.EndsWith("Mapper")),
                Implemetation = x
            })
            .Where(x => x.Interface is not null);

        foreach (var mapper in mappers)
        {
            services.AddSingleton(mapper.Interface!, mapper.Implemetation);
        }

        return services;
    }
}