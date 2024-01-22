using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace CandyKingdom.Marcy.Storage;

public sealed class S3FileStorage : IFileStorage, IDisposable
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;
    private readonly string _host;


    public S3FileStorage(S3FileStorageConfig config)
    {
        _bucketName = config.BucketName;
        _host = config.Host;

        var credentials = new BasicAWSCredentials
          (
                accessKey: config.ObjectStorageKeyId,
                secretKey: config.ObjectStorageKey
          );

        var conf = new AmazonS3Config()
        {
            ServiceURL = config.Host,
        };

        _s3Client = new AmazonS3Client(credentials, conf);
    }

    public async Task<StoredFile> Store(Stream stream, string name, string mimeType, CancellationToken cancellationToken = default)
    {
        var fileTransferUtility = new TransferUtility(_s3Client);

        var fileKey = $"{name}";

        var fileTransferUtilityRequest = new TransferUtilityUploadRequest
        {
            BucketName = _bucketName,
            InputStream = stream,
            StorageClass = S3StorageClass.Standard,
            PartSize = 6291456, // 6 MB.
            Key = fileKey,
            CannedACL = S3CannedACL.PublicRead,
            ContentType = mimeType
        };

        fileTransferUtilityRequest.Headers.CacheControl = "public, max-age=31557600";

        await fileTransferUtility.UploadAsync(fileTransferUtilityRequest, cancellationToken);

        var storedFile = new StoredFile
        {
            Url = $"{_host}/{_bucketName}/{fileKey}"
        };

        Console.WriteLine(storedFile.Url); // todo: remove

        return storedFile;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
