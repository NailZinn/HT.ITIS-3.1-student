using Dotnet.Homeworks.Shared.RabbitMqConfiguration;
using MassTransit;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        var host = rabbitConfiguration.GetHost();

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((ctx, rabbitConfigurator) =>
            {
                rabbitConfigurator.Host(host);
                rabbitConfigurator.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}