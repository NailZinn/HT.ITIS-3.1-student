using Dotnet.Homeworks.MainProject.Configuration;
using MassTransit;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        var hostname = $"amqp://{rabbitConfiguration.Username}:{rabbitConfiguration.Password}@{rabbitConfiguration.Hostname}:5672";

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((ctx, rabbitConfigurator) =>
            {
                rabbitConfigurator.Host(hostname);
                rabbitConfigurator.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}