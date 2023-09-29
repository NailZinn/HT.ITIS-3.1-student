using Dotnet.Homeworks.Mailing.API.Consumers;
using Dotnet.Homeworks.Shared.RabbitMqConfiguration;
using MassTransit;

namespace Dotnet.Homeworks.Mailing.API.ServicesExtensions;

public static class AddMasstransitRabbitMqExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        var host = rabbitConfiguration.GetHost();

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<EmailConsumer>();
            configurator.UsingRabbitMq((ctx, rabbitConfigurator) =>
            {
                rabbitConfigurator.Host(host);
                rabbitConfigurator.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}