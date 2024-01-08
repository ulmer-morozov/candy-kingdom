namespace CandyKingdom.Marcy.Storage;

public sealed record S3FileStorageConfig : ConfigurationBase
{
    public required string Host { get; init; }
    public required string BucketName { get; init; }
    public required string ObjectStorageKeyId { get; init; }
    public required string ObjectStorageKey { get; init; }

    public override void Verify()
    {
        ThrowIfNullOrWhiteSpace(Host, $"{nameof(Host)} can not be empty");
        ThrowIfNullOrWhiteSpace(BucketName, $"{nameof(BucketName)} can not be empty");
        ThrowIfNullOrWhiteSpace(ObjectStorageKeyId, $"{nameof(ObjectStorageKeyId)} can not be empty");
        ThrowIfNullOrWhiteSpace(ObjectStorageKey, $"{nameof(ObjectStorageKey)} can not be empty");
    }
}
