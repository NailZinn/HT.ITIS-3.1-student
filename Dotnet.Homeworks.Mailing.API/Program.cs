using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Services;
using Dotnet.Homeworks.Mailing.API.ServicesExtensions;
using Dotnet.Homeworks.Shared.RabbitMqConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));

builder.Services.AddScoped<IMailingService, MailingService>();

var rabbitMqConfig = new RabbitMqConfig
{
    Username = builder.Configuration["RabbitMQSettings:Username"]!,
    Password = builder.Configuration["RabbitMQSettings:Password"]!,
    Hostname = builder.Configuration["RabbitMQSettings:Hostname"]!,
    Port = int.Parse(builder.Configuration["RabbitMQSettings:Port"]!)
};

builder.Services.AddMasstransitRabbitMq(rabbitMqConfig);

var app = builder.Build();

app.Run();