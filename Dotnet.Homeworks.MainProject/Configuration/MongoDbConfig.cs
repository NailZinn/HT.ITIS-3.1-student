namespace Dotnet.Homeworks.MainProject.Configuration;

public class MongoDbConfig
{
    public string ConnectionString { get; set; } = default!;

    public string DatabaseName { get; set; } = default!;

    public string CollectionName { get; set; } = default!;
}