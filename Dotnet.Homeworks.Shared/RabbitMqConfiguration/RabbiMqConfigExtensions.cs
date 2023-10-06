namespace Dotnet.Homeworks.Shared.RabbitMqConfiguration;

public static class RabbiMqConfigExtensions
{
    public static string GetHost(this RabbitMqConfig config)
    {
        return $"amqp://{config.Username}:{config.Password}@{config.Hostname}:{config.Port}";
    }
}