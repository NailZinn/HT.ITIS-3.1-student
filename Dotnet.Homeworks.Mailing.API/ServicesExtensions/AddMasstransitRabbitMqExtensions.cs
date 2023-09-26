using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Consumers;
using MassTransit;

namespace Dotnet.Homeworks.Mailing.API.ServicesExtensions;

public static class AddMasstransitRabbitMqExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        var hostname = $"amqp://{rabbitConfiguration.Username}:{rabbitConfiguration.Password}@{rabbitConfiguration.Hostname}:5672";

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<EmailConsumer>();
            configurator.UsingRabbitMq((ctx, rabbitConfigurator) =>
            {
                rabbitConfigurator.Host(hostname);
                rabbitConfigurator.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}