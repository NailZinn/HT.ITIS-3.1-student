using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.MainProject.Configuration;
using MongoDB.Driver;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoClient(this IServiceCollection services, MongoDbConfig mongoConfiguration)
    {
        var client = new MongoClient(mongoConfiguration.ConnectionString);
        var database = client.GetDatabase(mongoConfiguration.DatabaseName);
        var collection = database.GetCollection<Order>(mongoConfiguration.CollectionName);

        services.AddTransient<IOrderRepository, OrderRepository>(_ => new OrderRepository(collection));
        
        return services;
    }
}