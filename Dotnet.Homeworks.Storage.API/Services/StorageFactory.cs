using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;
using Minio.DataModel.Args;

namespace Dotnet.Homeworks.Storage.API.Services;

public class StorageFactory : IStorageFactory
{
    private readonly IMinioClient _minioClient;

    public StorageFactory(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<IStorage<Image>> CreateImageStorageWithinBucketAsync(string bucketName)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);
        var bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs);
        
        if (!bucketExists)
        {
            var args = new MakeBucketArgs()
                .WithBucket(bucketName);

            await _minioClient.MakeBucketAsync(args);
        }
        
        return new ImageStorage(_minioClient, bucketName);
    }
}