using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Features;
using Dotnet.Homeworks.MainProject.Services;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;
using Dotnet.Homeworks.Shared.RabbitMqConfiguration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton<IRegistrationService, RegistrationService>();
builder.Services.AddSingleton<ICommunicationService, CommunicationService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddApplicationServices();

var rabbitMqConfig = new RabbitMqConfig
{
    Username = builder.Configuration["RabbitMQSettings:Username"]!,
    Password = builder.Configuration["RabbitMQSettings:Password"]!,
    Hostname = builder.Configuration["RabbitMQSettings:Hostname"]!,
    Port = int.Parse(builder.Configuration["RabbitMQSettings:Port"]!)
};

builder.Services.AddMasstransitRabbitMq(rabbitMqConfig);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapControllers();

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

await dbContext.Database.MigrateAsync();

app.Run();