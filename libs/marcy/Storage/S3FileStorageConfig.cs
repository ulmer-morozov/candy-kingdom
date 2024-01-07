namespace CandyKingdom.Marcy.Storage;

public sealed class S3FileStorageConfig : ConfigurationBase
{
    public required string Host { get; set; }
    public required string BucketName { get; set; }
    public required string ObjectStorageKeyId { get; set; }
    public required string ObjectStorageKey { get; set; }

    public override void Verify()
    {
        ThrowIfNullOrWhiteSpace(Host, $"{nameof(Host)} can not be empty");
        ThrowIfNullOrWhiteSpace(BucketName, $"{nameof(BucketName)} can not be empty");
        ThrowIfNullOrWhiteSpace(ObjectStorageKeyId, $"{nameof(ObjectStorageKeyId)} can not be empty");
        ThrowIfNullOrWhiteSpace(ObjectStorageKey, $"{nameof(ObjectStorageKey)} can not be empty");
    }
}
