using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.MainProject.Helpers;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.OpenTelemetry;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services,
        OpenTelemetryConfig openTelemetryConfiguration)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(AssemblyReference.Assembly.GetName().Name!);
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddJaegerExporter()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(openTelemetryConfiguration.OtlpExporterEndpoint);
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter();
            });
        
        return services;
    }
}