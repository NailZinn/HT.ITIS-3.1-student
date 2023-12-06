using System.Reactive.Linq;
using Dotnet.Homeworks.Shared.Dto;
using Dotnet.Homeworks.Storage.API.Constants;
using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Dotnet.Homeworks.Storage.API.Services;

public class ImageStorage : IStorage<Image>
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public ImageStorage(IMinioClient minioClient, string bucketName)
    {
        _minioClient = minioClient;
        _bucketName = bucketName;
    }

    public async Task<Result> PutItemAsync(Image item, CancellationToken cancellationToken = default)
    {
        var existingItem = await GetItemAsync(item.FileName, cancellationToken);

        if (existingItem is not null)
        {
            return $"Item {item.FileName} already exists";
        }
        
        item.Metadata.Add(MetadataKeys.Destination, _bucketName);

        var args = new PutObjectArgs()
            .WithBucket(Buckets.Pending)
            .WithObject(item.FileName)
            .WithObjectSize(item.Content.Length)
            .WithContentType(item.ContentType)
            .WithHeaders(item.Metadata)
            .WithStreamData(item.Content);

        var response = await _minioClient.PutObjectAsync(args, cancellationToken);

        return response is not null
            ? true
            : $"Couldn't put item {item.FileName} to bucket {Buckets.Pending}";
    }

    public async Task<Image?> GetItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(itemName)
            .WithCallbackStream(s => s.CopyToAsync(stream, cancellationToken));

        try
        {
            var response = await _minioClient.GetObjectAsync(args, cancellationToken);
            stream.Position = 0;

            return new Image(stream, response.ObjectName, response.ContentType, response.MetaData);
        }
        catch (BucketNotFoundException)
        {
            return null;
        }
        catch (ObjectNotFoundException)
        {
            return null;
        }
    }

    public async Task<Result> RemoveItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(itemName);

        try
        {
            await _minioClient.RemoveObjectAsync(args, cancellationToken);

            return true;
        }
        catch (BucketNotFoundException ex)
        {
            return ex.Message;
        }
    }

    public async Task<IEnumerable<string>> EnumerateItemNamesAsync(CancellationToken cancellationToken = default)
    {
        var args = new ListObjectsArgs()
            .WithBucket(_bucketName);

        return await _minioClient
            .ListObjectsAsync(args, cancellationToken)
            .Select(x => x.Key)
            .ToList();
    }

    public async Task<Result> CopyItemToBucketAsync(string itemName, string destinationBucketName,
        CancellationToken cancellationToken = default)
    {
        var args = new CopyObjectArgs()
            .WithBucket(destinationBucketName)
            .WithObject(itemName)
            .WithCopyObjectSource(new CopySourceObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(itemName));

        try
        {
            await _minioClient.CopyObjectAsync(args, cancellationToken);

            return true;
        }
        catch (BucketNotFoundException ex)
        {
            return ex.Message;
        }
        catch (ObjectNotFoundException ex)
        {
            return ex.Message;
        }
    }
}