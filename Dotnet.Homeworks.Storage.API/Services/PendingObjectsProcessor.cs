using Dotnet.Homeworks.Storage.API.Constants;

namespace Dotnet.Homeworks.Storage.API.Services;

public class PendingObjectsProcessor : BackgroundService
{
    private readonly IStorageFactory _storageFactory;

    public PendingObjectsProcessor(IStorageFactory storageFactory)
    {
        _storageFactory = storageFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var storage = await _storageFactory.CreateImageStorageWithinBucketAsync(Buckets.Pending);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var itemNames = await storage.EnumerateItemNamesAsync(stoppingToken);

                foreach (var itemName in itemNames)
                {
                    var item = await storage.GetItemAsync(itemName, stoppingToken);

                    if (item is null)
                    {
                        continue;
                    }

                    if (item.Metadata.TryGetValue(MetadataKeys.Destination, out var bucketName))
                    {
                        await storage.CopyItemToBucketAsync(itemName, bucketName, stoppingToken);
                    }
                    await storage.RemoveItemAsync(itemName, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            await Task.Delay(PendingObjectProcessor.Period, stoppingToken);
        }
    }
}